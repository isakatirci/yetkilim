using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Global;
using Yetkilim.Web.Models;

namespace Yetkilim.Web.Controllers
{
    [Route("auth")]
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;

        public AuthController(ILogger<AuthController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Route("signin")]
        public IActionResult SignIn(string returnUrl, [FromQuery(Name = "fr")] bool isFeedbackRedirect)
        {
            var localReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Action("Index", "Home");

            if (CurrentUser != null)
                return Redirect(localReturnUrl);

            var model = new LoginViewModel()
            {
                ReturnUrl = localReturnUrl,
                IsFeedbackRedirect = isFeedbackRedirect
            };

            return View(model);
        }

        [Route("signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var userRes = await _userService.GetUserAsync(model.Email, model.Password);
                if (!userRes.IsSuccess)
                {
                    model.FormMessage = userRes.FormMessage;
                    return View(model);
                }

                var user = userRes.Data;


                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, Consts.UserRoles.Member)
                };

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, Consts.Authentication.IdentityType));

                //var authRes = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                //var claimsPrincipal = new ClaimsPrincipal(authRes.Ticket.Principal.Identity);
                //claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, Consts.Authentication.IdentityType));

                var authProps = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc =
                        model.IsRemember ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(2),
                };


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProps);

                if (Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignIn Post Error");
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return View(model);
            }
        }

        [Route("signin/{provider}")]
        public IActionResult SignIn(string provider, string returnUrl = null)
        {
            return Challenge(
                new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallback", new { returnUrl }) },
                provider);
        }

        [Route("signup")]
        public IActionResult SignUp()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        [Route("signup")]
        [HttpPost]
        public async Task<ViewResult> SignUp(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Password != model.RePassword)
            {
                model.FormMessage = "Şifre ve şifre tekrarı aynı olmalıdır.";
                return View();
            }

            try
            {
                var userDto = new UserDTO()
                {
                    Name = model.FullName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Password = model.Password
                };

                var userRes = await _userService.AddUserAsync(userDto);

                if (!userRes.IsSuccess)
                {
                    model.FormMessage = userRes.FormMessage;
                    return View(model);
                }

                model.IsSuccess = true;
                model.FormMessage = "İşleminiz başarılı bir şekilde gerçekleştirildi.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignUp Post Error");

                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return View(model);
            }

            return View(model);
        }


        [AllowAnonymous]
        [HttpGet(nameof(ExternalLoginCallback))]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
         
            //Here we can retrieve the claims
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var signInModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                FormMessage = "İşleminiz gerçekleştirilirken sorun oluştu."
            };

            // read external identity from the temporary cookie
            if (result?.Succeeded != true)
                return View("SignIn", signInModel);

            // retrieve claims of the external user
            var externalUser = result.Principal;
            if (externalUser == null)
                return View("SignIn", signInModel);


            // retrieve claims of the external user
            var claims = externalUser.Claims.ToList();

            // try to determine the unique id of the external user - the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                signInModel.FormMessage = "Kullanıcı bilgilerinize erişelemedi. Yetkilim uygulamasına erişim verdiğinize emin olup tekrar deneyin.";
                return View("SignIn", signInModel);
            }

            var emailClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                signInModel.FormMessage = "E-Posta bilgilerinize erişelemedi. Yetkilim uygulamasının e-posta adresinize erişimine izin verdiğinize emin olup tekrar deneyin.";
                return View("SignIn", signInModel);
            }

            var nameClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            var externalUserId = userIdClaim.Value;
            var externalProvider = userIdClaim.Issuer;
            var userId = 0;

            try
            {
                var userRes = await _userService.GetExternalUserAsync(externalProvider, externalUserId);
                if (!userRes.IsSuccess)
                {
                    if (userRes.ResultCode == "NOT_REGISTERED")
                    {
                        var userDto = new UserDTO()
                        {
                            Name = nameClaim?.Value,
                            Email = emailClaim.Value
                        };

                        var registerRes = await _userService.AddExternalUserAsync(externalProvider, externalUserId, userDto);
                        if (!registerRes.IsSuccess)
                        {
                            signInModel.FormMessage = registerRes.FormMessage;
                            return View("SignIn", signInModel);
                        }

                        userId = registerRes.Data.Id;
                    }
                    else
                    {
                        signInModel.FormMessage = userRes.FormMessage;
                        return View("SignIn", signInModel);
                    }
                }
                else
                {
                    userId = userRes.Data.Id;
                }

                var extraClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    nameClaim,
                    new Claim(ClaimTypes.Role, Consts.UserRoles.Member)
                };

                var claimsPrincipal = new ClaimsPrincipal(result.Ticket.Principal.Identity);
                claimsPrincipal.AddIdentity(new ClaimsIdentity(extraClaims, Consts.Authentication.IdentityType));

                var props = new AuthenticationProperties { IsPersistent = true };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, props);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Facebook Post Error");

                return View("SignIn", signInModel);
            }

            return LocalRedirect(returnUrl ?? "/");
        }


        [Route("signout")]
        //[HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}