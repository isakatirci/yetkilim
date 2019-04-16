// BaseController
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using Yetkilim.Business.Services;
using Yetkilim.Global.Model;
using Yetkilim.Web.Models;
namespace Yetkilim.Web.Controllers
{
    public class BaseController : Controller
    {
        private CookieUserModel _user;

        public CookieUserModel CurrentUser
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
                _user = new CookieUserModel
                {
                    UserId = result,
                    FullName = fullName
                };
                return _user;
            }
        }



        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //IL_00ce: Unknown result type (might be due to invalid IL or missing references)
            //IL_00d3: Unknown result type (might be due to invalid IL or missing references)
            //IL_00da: Unknown result type (might be due to invalid IL or missing references)
            //IL_00e1: Unknown result type (might be due to invalid IL or missing references)
            //IL_010d: Expected O, but got Unknown
            if (CurrentUser != null)
            {
                HttpContext httpContext = context.HttpContext;
                string text = httpContext.Request.Cookies["pc"];
                if (text != null)
                {
                    ((dynamic)this.ViewBag).UserPromotionCount = text;
                }
                else
                {
                    IPromotionService service = ServiceProviderServiceExtensions.GetService<IPromotionService>(httpContext.RequestServices);
                    Result<int> result = service.GetUserActivePromotionCount(CurrentUser.UserId).Result;
                    int data = result.Data;
                    IResponseCookies cookies = httpContext.Response.Cookies;
                    string text2 = data.ToString();
                    CookieOptions val = new CookieOptions();
                    val.HttpOnly=(true);
                    val.Secure=(true);
                    val.Expires=((DateTimeOffset?)DateTime.Now.AddMinutes(1.0));
                    cookies.Append("pc", text2, val);
                    ((dynamic)this.ViewBag).UserPromotionCount = data.ToString();
                }
            }
            base.OnActionExecuting(context);
        }

        //public BaseController()
        //    : this()
        //{
        //}
    }
}
