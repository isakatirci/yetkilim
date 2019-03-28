// HomeController
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Entity;
using Yetkilim.Global.Model;
using Yetkilim.Web.Controllers;
using Yetkilim.Web.Models;
using Yetkilim.Web.Models.Ef;

public class HomeController : BaseController
{
    private readonly IPlaceService _placeService;

    private readonly IFeedbackService _feedbackService;

    private readonly ILogger _logger;

    private readonly IHostingEnvironment _hostingEnvironment;
    private readonly ICompanyService _companyService;
    private readonly ICompanyFeedbackService _companyFeedbackService;
    

    public HomeController(ILogger<HomeController> logger, IHostingEnvironment hostingEnvironment, IPlaceService placeService, IFeedbackService feedbackService, ICompanyService companyService, ICompanyFeedbackService companyFeedbackService)
    {
        _logger = logger;
        _hostingEnvironment = hostingEnvironment;
        _placeService = placeService;
        _feedbackService = feedbackService;
        _companyService = companyService;
        _companyFeedbackService = companyFeedbackService;
    }

    public IActionResult Index()
    {
        return this.View();
    }

    public async Task<IActionResult> Search(PlaceSearchRequestModel searchRequest)
    {
        PlaceSearchResultViewModel model = new PlaceSearchResultViewModel();
        if (searchRequest.Longitude.HasValue || searchRequest.Longitude.HasValue || !string.IsNullOrWhiteSpace(searchRequest.SearchText))
        {
            try
            {
                Result<List<PlaceDTO>> result = await _placeService.GetAllPlaceAsync(searchRequest.ToPlaceSearchModel());
                if (result.IsSuccess)
                {
                    model.Places = result.Data;
                }
                model.FormMessage = result.FormMessage;
                model.SearchText = searchRequest.SearchText;
                return this.View((object)model);
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "Search Error", Array.Empty<object>());
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return this.View((object)model);
            }
        }
        return this.View((object)model);
    }

    public ViewResult Feedback(int id)
    {
        FeedbackViewModel feedbackViewModel = new FeedbackViewModel
        {
            IsLogged = (base.CurrentUser != null)
        };
        try
        {
            var company = _companyService.GetCompanyAsync(id).Result.Data;

            if (string.Equals(company.Demo,"Evet",StringComparison.InvariantCultureIgnoreCase))
            {
                ViewBag.WarningDemo = "Dikkat bu işletme üyemiz değildir. Yine de değerlendirmenizi sorumlulara iletmeye çalışacağız. Dönüş geç yapılabilir ya da yapılmayabilir.";
            }


            using (yetkilimDBContext db = new yetkilimDBContext())
            {
                var companyFeedback = db.CompanyFeedback.First(x => x.TypeId == company.CompanyTypeId);
                return View("~/Views/Feedback/Feedback" + companyFeedback.FeedbackId + ".cshtml");
            }

            //var companyFeedback = _companyFeedbackService.GetCompanyFeedbackQueryable().First(x => x.TypeId == company.CompanyTypeId);
           
            //feedbackViewModel.FormId = 1;
        }
        catch (Exception ex)
        {
            LoggerExtensions.LogError(_logger, ex, "Feedback Error - PlaceID: {0}", new object[1]
            {
                id
            });
            feedbackViewModel.FormMessage = "İşleminiz gerçekleştirilemedi. Şirket tipine için Feedback sayfası eşleştirilemedi";
        }
        return this.View((object)feedbackViewModel);
    }

    [HttpPost]
    public async Task<JsonResult> Feedback(int id, FeedbackRequestModel model)
    {
        try
        {
            FeedbackDTO feedbackDTO = Mapper.Map<FeedbackRequestModel, FeedbackDTO>(model);
            if (base.CurrentUser != null)
            {
                feedbackDTO.UserId = base.CurrentUser.UserId;
            }
            feedbackDTO.PlaceId = id;
            try
            {
                feedbackDTO.IpAddress = this.Request.HttpContext.Connection
                    .RemoteIpAddress
                    .ToString();
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "IP Address error", Array.Empty<object>());
            }
            return this.Json((object)(await _feedbackService.AddFeedbackAsync(feedbackDTO)));
        }
        catch (Exception ex2)
        {
            return this.Json((object)Result.Fail("İşleminiz gerçekleştirilemedi", ex2));
        }
    }

    public IActionResult About()
    {
        ViewData["Message"] = "Your application description page.";
        return this.View();
    }

    public IActionResult Contact()
    {
        this.ViewData["Message"] = "Your contact page.";
        return this.View();
    }

    public IActionResult Privacy()
    {
        return this.View();
    }

    [ResponseCache(/*Could not decode attribute arguments.*/)]
    public IActionResult Error(int code = 0)
    {
        if (code == 404)
        {
            return this.View("Error404");
        }
        return this.View((object)new ErrorViewModel
        {
            RequestId = (Activity.Current?.Id ?? this.HttpContext.TraceIdentifier)
        });
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

