// AuthController
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Global.Model;
using Yetkilim.Web.Controllers;
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
            string text = this.Url.IsLocalUrl(returnUrl) ? returnUrl : UrlHelperExtensions.Action(this.Url, "Index", "Home");
            if (base.CurrentUser != null)
            {
                return this.Redirect(text);
            }
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = text,
                IsFeedbackRedirect = isFeedbackRedirect
            };
            return this.View((object)loginViewModel);
        }

        [Route("signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    Result<UserDTO> result = await _userService.GetUserAsync(model.Email, model.Password);
                    if (!result.IsSuccess)
                    {
                        model.FormMessage = result.FormMessage;
                        return this.View((object)model);
                    }
                    UserDTO data = result.Data;
                    List<Claim> claims = new List<Claim>
                {
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", data.Id.ToString()),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", data.Name),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", data.Email),
                    new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Member")
                };
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "ClaimIdentity"));
                    AuthenticationProperties val = new AuthenticationProperties();
                    val.IsPersistent=(true);
                    val.ExpiresUtc=((DateTimeOffset?)(model.IsRemember ? DateTimeOffset.UtcNow.AddDays(7.0) : DateTimeOffset.UtcNow.AddHours(2.0)));
                    AuthenticationProperties val2 = val;
                    await AuthenticationHttpContextExtensions.SignInAsync(this.HttpContext, "Cookies", claimsPrincipal, val2);
                    if (this.Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return this.Redirect(model.ReturnUrl);
                    }
                    return this.RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "SignIn Post Error", Array.Empty<object>());
                    model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                    return this.View((object)model);
                }
            }
            return this.View((object)model);
        }

        [Route("signin/{provider}")]
        public IActionResult SignIn(string provider, string returnUrl = null)
        {
            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
            //IL_0006: Unknown result type (might be due to invalid IL or missing references)
            //IL_0031: Expected O, but got Unknown
            AuthenticationProperties val = new AuthenticationProperties();
            val.RedirectUri=(UrlHelperExtensions.Action(this.Url, "ExternalLoginCallback", (object)new
            {
                returnUrl
            }));
            return this.Challenge(val, new string[1]
            {
            provider
            });
        }

        [Route("signup")]
        public IActionResult SignUp()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            return this.View((object)registerViewModel);
        }

        [Route("signup")]
        [HttpPost]
        public async Task<ViewResult> SignUp(RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View((object)model);
            }
            if (model.Password != model.RePassword)
            {
                model.FormMessage = "Şifre ve şifre tekrarı aynı olmalıdır.";
                return this.View();
            }
            try
            {
                UserDTO model2 = new UserDTO
                {
                    Name = model.FullName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Password = model.Password
                };
                Result<UserDTO> result = await _userService.AddUserAsync(model2);
                if (!result.IsSuccess)
                {
                    model.FormMessage = result.FormMessage;
                    return this.View((object)model);
                }
                model.IsSuccess = true;
                model.FormMessage = "İşleminiz başarılı bir şekilde gerçekleştirildi.";
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "SignUp Post Error", Array.Empty<object>());
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return this.View((object)model);
            }
            return this.View((object)model);
        }

        [AllowAnonymous]
        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            AuthenticateResult result = await AuthenticationHttpContextExtensions.AuthenticateAsync(this.HttpContext, "Cookies");
            LoginViewModel signInModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                FormMessage = "İşleminiz gerçekleştirilirken sorun oluştu."
            };
            AuthenticateResult obj = result;
            if (obj == null || !obj.Succeeded)
            {
                return this.View("SignIn", (object)signInModel);
            }
            ClaimsPrincipal principal = result.Principal;
            if (principal == null)
            {
                return this.View("SignIn", (object)signInModel);
            }
            List<Claim> source = principal.Claims.ToList();
            Claim claim = source.FirstOrDefault((Claim x) => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (claim == null)
            {
                signInModel.FormMessage = "Kullanıcı bilgilerinize erişelemedi. Yetkilim uygulamasına erişim verdiğinize emin olup tekrar deneyin.";
                return this.View("SignIn", (object)signInModel);
            }
            string externalProvider = claim.Issuer;
            Claim emailClaim = null;
            if (externalProvider == "Facebook")
            {
                emailClaim = source.FirstOrDefault((Claim x) => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                if (emailClaim == null)
                {
                    signInModel.FormMessage = "E-Posta bilgilerinize erişelemedi. Yetkilim uygulamasının e-posta adresinize erişimine izin verdiğinize emin olup tekrar deneyin.";
                    return this.View("SignIn", (object)signInModel);
                }
            }
            Claim nameClaim = source.FirstOrDefault((Claim x) => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            string externalUserId = claim.Value;
            try
            {
                Result<UserDTO> result2 = await _userService.GetExternalUserAsync(externalProvider, externalUserId);
                int ıd;
                if (!result2.IsSuccess)
                {
                    if (!(result2.ResultCode == "NOT_REGISTERED"))
                    {
                        signInModel.FormMessage = result2.FormMessage;
                        return this.View("SignIn", (object)signInModel);
                    }
                    UserDTO model = new UserDTO
                    {
                        Name = nameClaim?.Value,
                        Email = emailClaim?.Value
                    };
                    Result<UserDTO> result3 = await _userService.AddExternalUserAsync(externalProvider, externalUserId, model);
                    if (!result3.IsSuccess)
                    {
                        signInModel.FormMessage = result3.FormMessage;
                        return this.View("SignIn", (object)signInModel);
                    }
                    ıd = result3.Data.Id;
                }
                else
                {
                    ıd = result2.Data.Id;
                }
                List<Claim> claims = new List<Claim>
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", ıd.ToString()),
                nameClaim,
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Member")
            };
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(result.Ticket.Principal.Identity);
                claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, "ClaimIdentity"));
                AuthenticationProperties val = new AuthenticationProperties();
                val.IsPersistent=(true);
                await AuthenticationHttpContextExtensions.SignInAsync(this.HttpContext, "Cookies", claimsPrincipal, val);
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "Facebook Post Error", Array.Empty<object>());
                return this.View("SignIn", (object)signInModel);
            }
            return this.LocalRedirect(returnUrl ?? "/");
        }

        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            await AuthenticationHttpContextExtensions.SignOutAsync(this.HttpContext, "Cookies");
            return this.RedirectToAction("Index", "Home");
        }
    }
}

