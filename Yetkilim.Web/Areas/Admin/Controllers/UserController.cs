using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Yetkilim.Web.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}