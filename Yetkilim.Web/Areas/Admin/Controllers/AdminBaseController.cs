using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yetkilim.Global;

namespace Yetkilim.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = Consts.AdminArea.AuthenticationScheme, Roles = Consts.AdminArea.BaseRole)]
    public class AdminBaseController : Controller
    {
    }
}