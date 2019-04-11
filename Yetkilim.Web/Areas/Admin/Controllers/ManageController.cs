using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Enums;
using Yetkilim.Global.Model;
using Yetkilim.Web.Areas.Admin.Models;

namespace Yetkilim.Web.Areas.Admin.Controllers
{
    public class ManageController : AdminBaseController
    {
        private readonly ILogger<ManageController> _logger;

        private readonly IFeedbackService _feedbackService;

        private readonly IPlaceService _placeService;

        private readonly IPanelUserService _panelUserService;

        public ManageController(ILogger<ManageController> logger, IFeedbackService feedbackService, IPlaceService placeService, IPanelUserService panelUserService)
        {
            _logger = logger;
            _feedbackService = feedbackService;
            _placeService = placeService;
            _panelUserService = panelUserService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                DashboardViewModel model = new DashboardViewModel();
                int? companyId = base.CurrentUser.CompanyId;
                int? num = base.CurrentUser.PlaceId;
                if (base.CurrentUser.Role == UserRole.SuperAdmin)
                {
                    companyId = null;
                    num = null;
                }
                CompanyUserSearchModel feedbackSearchModel = new CompanyUserSearchModel
                {
                    CompanyId = companyId,
                    PlaceId = num,
                    Page = 1,
                    PageSize = 10
                };
                Result<int> feedbackCount = await _feedbackService.GetAllFeedbackCountAsync(companyId, num);
                Result<List<FeedbackDTO>> result = await _feedbackService.GetAllFeedbackAsync(feedbackSearchModel);
                if (result.IsSuccess)
                {
                    model.Feedbacks = (from o in result.Data
                                       orderby o.CreatedDate descending
                                       select o).ToList();
                    model.FeedbackCount = feedbackCount.Data;
                }
                Result<int> result2 = await _placeService.GetPlaceCountByCompanyIdAsync(companyId);
                if (result2.IsSuccess)
                {
                    model.PlaceCount = result2.Data;
                }
                ((dynamic)ViewBag).CompanyName = base.CurrentUser.CompanyName;
                return View((object)model);
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "Panel Index Error", Array.Empty<object>());
                return this.RedirectToAction("SignOut");
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            AdminLoginViewModel adminLoginViewModel = new AdminLoginViewModel();
            return View((object)adminLoginViewModel);
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    Result<PanelUserDTO> result = await _panelUserService.GetUserAsync(model.Email, model.Password);
                    if (!result.IsSuccess)
                    {
                        model.FormMessage = "E-Posta ya da Şifre bilgisi yanlış, lütfen bilgilerinizi kontrol edin.";
                        return this.View((object)model);
                    }
                    PanelUserDTO data = result.Data;
                    List<Claim> claims = new List<Claim>
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", data.Id.ToString()),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", data.Name),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", data.Email),
                new Claim("CompanyId", data.CompanyId.ToString()),
                new Claim("PlaceId", data.PlaceId.ToString()),
                new Claim("CompanyName", data.Company.Name),
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", data.Role.ToString())
            };
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "ClaimIdentity"));
                   var task = AuthenticationHttpContextExtensions.SignInAsync(this.HttpContext, "AdminAreaCookies", claimsPrincipal);
                    task.Wait();
                    if (task.IsCompletedSuccessfully)
                    {
                      var temp =  this.User.Claims;
                    }
                    else
                    {

                    }                      
                    return this.RedirectToAction("Index", "Manage");
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "Panel Login Error", Array.Empty<object>());
                    model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                    return this.View((object)model);
                }
            }
            return this.View((object)model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            AdminLoginViewModel model = new AdminLoginViewModel();
            try
            {
                Result result = await _panelUserService.ForgotPasswordAsync(email);
                if (!result.IsSuccess)
                {
                    model.FormMessage = result.FormMessage;
                    return this.Json((object)model);
                }
                model.IsSuccessForgot = true;
                return this.Json((object)model);
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "Panel ForgotPassword Error", Array.Empty<object>());
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return this.Json((object)model);
            }
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return this.RedirectToAction("Login");
            }
            AdminUserResetPasswordModel adminUserResetPasswordModel = new AdminUserResetPasswordModel
            {
                Code = code
            };
            return View((object)adminUserResetPasswordModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(AdminUserResetPasswordModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    if (model.Password != model.PasswordRe)
                    {
                        model.FormMessage = "Şifre ve tekrarı aynı olmalıdır.";
                        return View((object)model);
                    }
                    Result result = await _panelUserService.ResetPasswordAsync(model.Code, model.Email, model.Password);
                    if (!result.IsSuccess)
                    {
                        model.FormMessage = result.FormMessage;
                        return View((object)model);
                    }
                    model.IsSuccess = true;
                    model.FormMessage = "Yeni şifreniz ile giriş yapabilirsiniz.";
                    return View((object)model);
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "Panel ResetPassword Error", Array.Empty<object>());
                    model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                    return View((object)model);
                }
            }
            return View((object)model);
        }

        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            await AuthenticationHttpContextExtensions.SignOutAsync(this.HttpContext, "AdminAreaCookies");
            return this.RedirectToAction("Login", "Manage");
        }   
    }
}