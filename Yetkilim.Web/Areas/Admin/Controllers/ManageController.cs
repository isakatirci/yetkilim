using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

        

        private string GetPlaceName(object id)
        {
            try
            {
                using (Yetkilim.Web.Models.Ef.yetkilimDBContext db = new Yetkilim.Web.Models.Ef.yetkilimDBContext())
                {
                    db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    db.ChangeTracker.AutoDetectChangesEnabled = false;
                    var id1 = (int)id;
                    var name = db.Places.AsNoTracking().First(x => x.Id == id1).Name;
                    return name;
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string myTableMaker<T>(T[] list, string id)
        {
            if (list.Length == 0)
            {
                return "";
            }
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);


            var propertiesList = new List<PropertyInfo>();
            var headers = Consts.Headers;
            var keys = headers.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                var prop = properties.FirstOrDefault(x => string.Equals(keys[i], x.Name, StringComparison.InvariantCultureIgnoreCase));
                if (prop != null)
                {
                    propertiesList.Add(prop);
                }
            }
            properties = propertiesList.ToArray();


            //Array.Sort(properties, new ComparerPropertyInfo());
            var table = "<table class=\"table\" id=\"data-table-feedback" + id + "\"><thead><tr>";
            for (int j = 0; j < properties.Length; j++)
            {
                if (string.IsNullOrWhiteSpace(headers[properties[j].Name]))
                {
                    continue;
                }
                table += "<th class=\"secondary-text\"><div class=\"table-header\"><span class=\"column-title\">" + headers[properties[j].Name] + "</span></div></td>";
            }
            table += "</tr></thead><tbody>";
            for (int i = 0; i < list.Length; i++)
            {
                table += "<tr>";

                var isUserShare = false;
                var anonimMi = false;
                try
                {
                    var isUserShareProp = properties.FirstOrDefault(x => string.Equals("IsUserShare", x.Name, StringComparison.InvariantCultureIgnoreCase));
                    if (isUserShareProp != null)                    {

                        var temp = (bool?)isUserShareProp.GetValue(list[i], null);
                        isUserShare = temp.HasValue && temp.Value;
                    }
                }
                catch (Exception)
                {

                    
                }                
                try
                {
                    var userIdProp = properties.FirstOrDefault(x => string.Equals("UserId", x.Name, StringComparison.InvariantCultureIgnoreCase));
                    if (userIdProp != null)
                    {
                        var temp = (int?)(userIdProp.GetValue(list[i], null));
                        anonimMi = temp.HasValue && temp < 1;
                    }
                }
                catch (Exception)
                {

                }



                for (int j = 0; j < properties.Length; j++)
                {
                    if (string.IsNullOrWhiteSpace(headers[properties[j].Name]))
                    {
                        continue;
                    }

                    if (string.Equals(properties[j].Name, "UserPhone", StringComparison.InvariantCultureIgnoreCase))
                    {
                        table += "<td>" + (anonimMi ? "Anonim" : (isUserShare ? properties[j].GetValue(list[i], null) : "Gizli") ) + "</td>";
                        continue;
                    }

                    if (string.Equals(properties[j].Name, "UserFullName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        table += "<td>" + (anonimMi ? "Anonim" : (isUserShare ? properties[j].GetValue(list[i], null) : "Gizli")) + "</td>";
                        continue;
                    }

                    if (string.Equals(properties[j].Name, "UserMail", StringComparison.InvariantCultureIgnoreCase))
                    {
                        table += "<td>" + (anonimMi ? "Anonim" : (isUserShare ? properties[j].GetValue(list[i], null) : "Gizli")) + "</td>";
                        continue;
                    }                  


                    if (string.Equals(properties[j].Name, "PlaceId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        table += "<td>" + GetPlaceName(properties[j].GetValue(list[i], null)) + "</td>";
                        continue;
                    }

                    table += "<td>" + properties[j].GetValue(list[i], null) + "</td>";
                }
                table += "</tr>";
            }
            table += "</tbody></table>";

            return table;
        }      
     

        public async Task<IActionResult> Index()
        {
            try
            {
                DashboardViewModel model = new DashboardViewModel();
                var table = string.Empty;
                var feedbackCount = 0;
                //var feedbackCount = 0;
                var isletmeAdi = string.Empty;

                using (Yetkilim.Web.Models.Ef.yetkilimDBContext db = new Yetkilim.Web.Models.Ef.yetkilimDBContext())
                {
                    db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    db.ChangeTracker.AutoDetectChangesEnabled = false;

                    var placesIds = new List<int?>();
                    if (base.CurrentUser.Role == UserRole.Admin)
                    {
                        var currentUserCompanyId = base.CurrentUser.CompanyId;
                        placesIds = db.Places.Where(x => x.CompanyId == currentUserCompanyId).Select(x => (int?)x.Id).ToList();
                    }                    
                    else if (base.CurrentUser.Role == UserRole.Dealer)
                    {
                        var currentPlaceId = base.CurrentUser.PlaceId;
                        placesIds = db.Places.Where(x => x.Id == currentPlaceId).Select(x => (int?)x.Id).ToList();
                        var place = db.Places.FirstOrDefault(x=>x.Id == currentPlaceId);
                        if (place != null)
                        {
                            isletmeAdi += place.Name + " ";
                        }

                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback0.AsNoTracking()
                            .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "0");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback1.AsNoTracking()
                            .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray(); ;
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "1");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback2.AsNoTracking()
                             .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "2");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback3.AsNoTracking()
                             .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "3");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback4.AsNoTracking()
                            .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "4");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback5.AsNoTracking()
                             .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "5");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback6.AsNoTracking()
                            .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "6");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback7.AsNoTracking()
                             .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "7");
                    }
                   model.PlaceCount = base.CurrentUser.Role != UserRole.SuperAdmin ? placesIds.Count : db.Places.Count(); 
                   model.FeedbackCount = feedbackCount;

                }
                
                ViewBag.Table = table;
                ((dynamic)ViewBag).CompanyName = isletmeAdi + base.CurrentUser.CompanyName;
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
                        var temp = this.User.Claims;
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