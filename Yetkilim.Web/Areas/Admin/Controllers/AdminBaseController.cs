using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Yetkilim.Domain.Enums;
using Yetkilim.Web.Areas.Admin.Models;

namespace Yetkilim.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAreaCookies", Roles = "SuperAdmin,Admin,Dealer")]
    public class AdminBaseController : Controller
    {
        private AdminCookieUserModel _user;

        public AdminCookieUserModel CurrentUser
        {
            get


            {
                if (this.User?.Identities == null || !this.User.Identities.Any())
                {
                    return null;
                }
                if (_user != null)
                {
                    return _user;
                }
                ClaimsIdentity claimsIdentity = this.User.Identities.FirstOrDefault((ClaimsIdentity w) => w.AuthenticationType == "ClaimIdentity");
                if (claimsIdentity == null)
                {
                    return null;
                }
                string s = claimsIdentity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                int.TryParse(s, out int result);
                string fullName = claimsIdentity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                string email = claimsIdentity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
                int.TryParse(claimsIdentity.FindFirst("CompanyId")?.Value, out int result2);
                int.TryParse(claimsIdentity.FindFirst("PlaceId")?.Value, out int result3);
                string companyName = claimsIdentity.FindFirst("CompanyName")?.Value;
                Enum.TryParse(claimsIdentity.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value, out UserRole result4);
                int? placeId = null;
                if (result3 > 0 && result4 == UserRole.Dealer)
                {
                    placeId = result3;
                }
                _user = new AdminCookieUserModel
                {
                    UserId = result,
                    Email = email,
                    FullName = fullName,
                    CompanyId = result2,
                    PlaceId = placeId,
                    CompanyName = companyName,
                    Role = result4
                };
                return _user;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ((dynamic)this.ViewBag).CurrentUser = CurrentUser;
            base.OnActionExecuting(context);
        }

        //public AdminBaseController()
        //    : this()
        //{
        //}
    }
}