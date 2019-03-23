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
using Yetkilim.Web.Models.Ef;

namespace Yetkilim.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPlaceService _placeService;
        private readonly IFeedbackService _feedbackService;
        private readonly ICompanyService _companyService;

        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment hostingEnvironment, IPlaceService placeService,
            IFeedbackService feedbackService, ICompanyService companyService)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _placeService = placeService;
            _feedbackService = feedbackService;
            _companyService = companyService;
        }

        public IActionResult Index()


        {
            //if (_hostingEnvironment.IsDevelopment())
            //{
            //    return RedirectToAction("Index", "Manage", new { area = "Admin" });
            //}

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
                var company = _companyService.GetCompanyAsync(id).Result.Data;              
                return View("~/Views/Feedback/Feedback" + company.CompanyTypeId + ".cshtml");
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

        public IActionResult Feedback0(Feedback0 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture); 
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback0.Add(feedback);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback1(Feedback1 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture); 
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback1.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback2(Feedback2 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture); 
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback2.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback3(Feedback3 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture);   
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback3.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback4(Feedback4 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture);  
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback4.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback5(Feedback5 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture);  
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback5.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback6(Feedback6 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture); 
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback6.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback7(Feedback7 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture);
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback7.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }


    }
}