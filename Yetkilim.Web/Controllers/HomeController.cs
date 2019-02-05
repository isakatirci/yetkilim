using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Global.Model;
using Yetkilim.Web.Models;

namespace Yetkilim.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPlaceService _placeService;
        private readonly IFeedbackService _feedbackService;

        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment hostingEnvironment, IPlaceService placeService,
            IFeedbackService feedbackService)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _placeService = placeService;
            _feedbackService = feedbackService;
        }

        public IActionResult Index()
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                return RedirectToAction("Index", "Manage", new { area = "Admin" });
            }

            return View();
        }

        public async Task<IActionResult> Search(PlaceSearchRequestModel searchRequest)
        {
            var model = new PlaceSearchResultViewModel();
            if (searchRequest.Longitude.HasValue == false && searchRequest.Longitude.HasValue == false && string.IsNullOrWhiteSpace(searchRequest.SearchText))
                return View(model);

            try
            {
                var placesResult = await _placeService.GetAllPlaceAsync(searchRequest.ToPlaceSearchModel());
                if (placesResult.IsSuccess)
                    model.Places = placesResult.Data;

                model.FormMessage = placesResult.FormMessage;
                model.SearchText = searchRequest.SearchText;

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Search Error");

                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return View(model);
            }
        }

        public ViewResult Feedback(int id)
        {
            var model = new FeedbackViewModel();
            try
            {
                //var formRes = await _placeService.GetCompanyFormAsync(id);
                //model.FormMessage = formRes.FormMessage;

                //if (formRes.IsSuccess)
                //{
                //    model.FormId = formRes.Data.Id;
                //    model.FormData = formRes.Data.FormItems;
                //}

                // TODO: şimdilik default form gösterilecek
                model.FormId = 1;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feedback Error - PlaceID: {0}", id);

                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
            }
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Feedback(int id, FeedbackRequestModel model)
        {
            // TODO: userId gönderilecek
            // TODO: login değilse email ve/veya telefon alınacak

            try
            {
                var feedback = Mapper.Map<FeedbackRequestModel, FeedbackDTO>(model);

                if (CurrentUser != null)
                    feedback.UserId = CurrentUser.UserId;

                feedback.PlaceId = id;
                try
                {
                    feedback.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "IP Address error");
                }
                var result = await _feedbackService.AddFeedbackAsync(feedback);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(Result.Fail("İşleminiz gerçekleştirilemedi", ex));
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}