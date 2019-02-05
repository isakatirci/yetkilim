using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Yetkilim.Global;
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
                if (User?.Identities == null || !User.Identities.Any()) return null;

                if (_user != null) return _user;

                var userClaim =
                    User.Identities.FirstOrDefault(w => w.AuthenticationType == Consts.Authentication.IdentityType);
                if (userClaim == null) return null;

                var strId = userClaim.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int.TryParse(strId, out var id);

                var fullName = userClaim.FindFirst(ClaimTypes.Name)?.Value;

                _user = new CookieUserModel
                {
                    UserId = id,
                    FullName = fullName
                };

                return _user;
            }
        }
    }
}