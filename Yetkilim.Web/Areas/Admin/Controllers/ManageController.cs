using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Global;
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

        public async Task<ViewResult> Index()
        {
            var model = new DashboardViewModel();

            var companyId = 71;
            int? placeId = null;

            var feedbackSearchModel = new FeedbackSearchModel()
            {
                CompanyId = companyId,
                PlaceId = placeId,
                Page = 1,
                PageSize = 10
            };


            var feedbackResult = await _feedbackService.GetAllFeedbackAsync(feedbackSearchModel);
            if (feedbackResult.IsSuccess)
            {
                var feedbacks = feedbackResult.Data;

                model.Feedbacks = feedbacks;
                model.FeedbackCount = feedbacks.Count;
            }


            var placeCountResult = await _placeService.GetPlaceCountByCompanyIdAsync(companyId);
            if (placeCountResult.IsSuccess)
                model.PlaceCount = placeCountResult.Data;

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            var model = new AdminLoginViewModel();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var userRes = await _panelUserService.GetUserAsync(model.Email, model.Password);
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
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                };

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

                await HttpContext.SignInAsync(Consts.AdminArea.AuthenticationScheme, claimsPrincipal);

                return RedirectToAction("Index", "Manage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Panel Login Error");
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";

                return View(model);
            }
        }

        [Route("signout")]
        //[HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(Consts.AdminArea.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}