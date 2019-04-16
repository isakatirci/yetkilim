// HomeController
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Entity;
using Yetkilim.Global.Model;
using Yetkilim.Infrastructure.Email;
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

    private readonly IEmailSender _emailSender;
    public HomeController(ILogger<HomeController> logger, IHostingEnvironment hostingEnvironment, IPlaceService placeService, IFeedbackService feedbackService, ICompanyService companyService, ICompanyFeedbackService companyFeedbackService, IEmailSender emailSender)
    {
        _logger = logger;
        _hostingEnvironment = hostingEnvironment;
        _placeService = placeService;
        _feedbackService = feedbackService;
        _companyService = companyService;
        _companyFeedbackService = companyFeedbackService;
        _emailSender = emailSender;
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
        FeedbacksViewModelBase feedbackViewModel = null;

        try
        {

            

            var place = _placeService.GetPlaceAsync(id).Result.Data;

            ViewBag.PlaceName = place.Name;
            ViewBag.PlaceId = place.Id;

            if (string.Equals(place.Guest,"Evet",StringComparison.InvariantCultureIgnoreCase))
            {
                ViewBag.WarningDemo = "Dikkat bu işletme üyemiz değildir. Yine de değerlendirmenizi sorumlulara iletmeye çalışacağız. Dönüş geç yapılabilir ya da yapılmayabilir.";
            }

            using (yetkilimDBContext db = new yetkilimDBContext())
            {

                var company = db.Companies.FirstOrDefault(x => x.Id == place.CompanyId);
                var companyFeedback = db.CompanyFeedback.FirstOrDefault(x => x.TypeId == company.CompanyTypeId);                
                if (companyFeedback != null)
                {
                    var tempFeedbackId = companyFeedback.FeedbackId;
                    switch (tempFeedbackId)
                    {
                        case 0:
                            feedbackViewModel = new FeedbackViewModel0();
                            break;
                        case 1:
                            feedbackViewModel = new FeedbackViewModel1();
                            break;
                        case 2:
                            feedbackViewModel = new FeedbackViewModel2();
                            break;
                        case 3:
                            feedbackViewModel = new FeedbackViewModel3();
                            break;
                        case 4:
                            feedbackViewModel = new FeedbackViewModel4();
                            break;
                        case 5:
                            feedbackViewModel = new FeedbackViewModel5();
                            break;
                        case 6:
                            feedbackViewModel = new FeedbackViewModel6();
                            break;
                        case 7:
                            feedbackViewModel = new FeedbackViewModel7();
                            break;
                    }
                    feedbackViewModel.IsLogged = (base.CurrentUser != null);
                    return View("~/Views/Feedback/Feedback" + companyFeedback.FeedbackId + ".cshtml", model: feedbackViewModel);

                }
                return View("~/Views/Feedback/Feedback" + 0 + ".cshtml", model: feedbackViewModel);
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
            ViewBag.WarningDemo = "İşleminiz gerçekleştirilemedi. Şirket tipine için Feedback sayfası eşleştirilemedi";
        }
        return View("~/Views/Home/Index.cshtml");
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

    Dictionary<string, string> headers = new Dictionary<string, string>()
    {

{"Id",""                                       } ,
{"CreatedDate","Tarih"                         } ,
{"AdviseRate",   "Tavsiye"                     } ,
{"EmployeeRate", "Personel"                    } ,
{"CleaningRate", "Hijyen"                      } ,
{"LikeRate", ""                    } ,
{"Description",  "Değerlendirme"               } ,
{"FlavorRate",   "Lezzet"                      } ,
{"PriceRate",    "Fiyat"                       } ,
{"DeskCode", "Masa Kodu"                       } ,
{"IsUserShare",""                              } ,
{"IpAddress",""                                } ,
{"FormValue",""                                } ,
{"FormId",""                                   } ,
{"UserId",   ""                                } ,
{"PlaceId",  "İşletme"                         } ,
{"DetailId",""                                 } ,
{"ModifiedDate",""                             } ,
{"CreatedBy",""                                } ,
{"ModifiedBy",""                               } ,
{"BrowserFp",""                                } ,
{"IsDeleted",    ""                            } ,
{"UserFullName",""                             } ,
{"UserMail",""                                 } ,
{"UserPhone",    ""                            } ,
{"UserAddress",""                              } ,
{"WCCleaningRate",   "WC Hijyen"               } ,
{"EmployeeSkill",    "Personel Yeteneği"       } ,
{"Ikram",    "İkram"                           } ,
{"Rotar",    "Rötar"                           } ,
{"SafeDrive",    "Güvenli Sürüş"               } ,
{"SeatNo",   "Koltuk"                          } ,
{"DoktorUzmanligi",  "Dr. Uzamanlık"           } ,
{"UzmanCesidi",  "Uzman Çeşidi"                } ,
{"ReyonGorevlisi",   "Reyon Görevlisi"         } ,
{"UrunCesidi",   "Ürün Çeşidi"                 } ,
{"EtkinlikAdi",  "Etkinlik Adı"                } ,
{"MekanYeterliligi", "Mekan Yeterliliği"       } ,
{"PlanaUyum",    "Plana Uyum"                  } ,


    };

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
        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
            for (int j = 0; j < properties.Length; j++)
            {
                if (string.IsNullOrWhiteSpace(headers[properties[j].Name]))
                {
                    continue;
                }

                if (string.Equals(properties[j].Name, "PlaceId", StringComparison.InvariantCultureIgnoreCase))
                {
                    table += "<td style=\"vertical-align: top;\">" + GetPlaceName(properties[j].GetValue(list[i], null)) + "</td>";
                    continue;
                }

                table += "<td style=\"vertical-align: top;\">" + properties[j].GetValue(list[i], null) + "</td>";
            }
            table += "</tr>";
        }
        table += "</tbody></table>";

        return table;
    }


    public IActionResult Feedback0(Feedback0 feedback)
    {
        try
        {
            //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture); 
            using (yetkilimDBContext db = new yetkilimDBContext())
            {
                var userId = 0;
                if (base.CurrentUser != null)
                {
                    userId = base.CurrentUser.UserId;
                }
                try
                {
                    if (feedback.IsUserShare.HasValue && feedback.IsUserShare.Value)
                    {
                        var user = db.Users.AsNoTracking().FirstOrDefault(x => x.Id == userId);
                        feedback.UserFullName = user.Name;
                        feedback.UserMail = user.Email;
                        feedback.UserPhone = user.Phone;
                    }
                
                    //if (!string.IsNullOrWhiteSpace(feedback.UserFullName)
                    //     || !string.IsNullOrWhiteSpace(feedback.UserMail)
                    //     || !string.IsNullOrWhiteSpace(feedback.UserPhone))
                    //{
                    //    Users model2 = new Users
                    //    {
                    //        Name = feedback.UserFullName,
                    //        Email = feedback.UserMail,
                    //        Phone = feedback.UserPhone,                            
                    //        Password = "",
                    //    };
                    //    db.Users.Add(model2);
                    //    db.SaveChanges();
                    //    userId = model2.Id;
                    //}                 
                }
                catch (Exception)
                {

                }
                feedback.CreatedDate = DateTime.Now;
                feedback.UserId = userId;

                try
                {
                    feedback.IpAddress = this.Request.HttpContext.Connection
                        .RemoteIpAddress
                        .ToString();
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "IP Address error", Array.Empty<object>());
                }
                db.Feedback0.Add(feedback);
                db.SaveChanges();
                try
                {

                    var listMail = new List<string>();

                    var userPlaces = db.PanelUser.AsNoTracking().Where(x => x.PlaceId == feedback.PlaceId && !x.IsDeleted).ToList();
                    if (userPlaces != null && userPlaces.Any())
                    {
                        listMail.AddRange(userPlaces.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    var place = db.Places.First(x => x.Id == feedback.PlaceId);

                    var userCompanies = db.PanelUser.AsNoTracking().Where(x => x.CompanyId == place.CompanyId && !x.IsDeleted).ToList();
                    if (userCompanies != null && userCompanies.Any())
                    {
                        listMail.AddRange(userCompanies.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    _emailSender.Send(
                   listMail.ToArray()
                    , "Müşteriniz işletmeniz için bir değerlendirme yaptı", myTableMaker(new Feedback0[] { feedback }, "0"));
                }
                catch (Exception)
                {


                }

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
                var userId = 0;
                if (base.CurrentUser != null)
                {
                    userId = base.CurrentUser.UserId;
                }
                try
                {
                    if (feedback.IsUserShare.HasValue && feedback.IsUserShare.Value)
                    {
                        var user = db.Users.AsNoTracking().FirstOrDefault(x => x.Id == userId);
                        feedback.UserFullName = user.Name;
                        feedback.UserMail = user.Email;
                        feedback.UserPhone = user.Phone;
                    }
                }
                catch (Exception)
                {

                }
                feedback.CreatedDate = DateTime.Now;
                feedback.UserId = userId;
                try
                {
                    feedback.IpAddress = this.Request.HttpContext.Connection
                        .RemoteIpAddress
                        .ToString();
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "IP Address error", Array.Empty<object>());
                }
                db.Feedback1.Add(feedback);
                db.SaveChanges();

                try
                {
                    var listMail = new List<string>();

                    var userPlaces = db.PanelUser.AsNoTracking().Where(x => x.PlaceId == feedback.PlaceId && !x.IsDeleted).ToList();
                    if (userPlaces != null && userPlaces.Any())
                    {
                        listMail.AddRange(userPlaces.Where(x=> !string.IsNullOrWhiteSpace(x.Email)).Select(x=>x.Email));
                    }

                    var place = db.Places.First(x => x.Id == feedback.PlaceId);

                    var userCompanies = db.PanelUser.AsNoTracking().Where(x => x.CompanyId == place.CompanyId && !x.IsDeleted).ToList();
                    if (userCompanies != null && userCompanies.Any())
                    {
                        listMail.AddRange(userCompanies.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    _emailSender.Send(                    
                   listMail.ToArray()
                    , "Müşteriniz işletmeniz için bir değerlendirme yaptı", myTableMaker(new Feedback1[] { feedback }, "1"));
                }
                catch (Exception ex)
                {


                }

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
                var userId = 0;
                if (base.CurrentUser != null)
                {
                    userId = base.CurrentUser.UserId;
                }
                try
                {
                    if (feedback.IsUserShare.HasValue && feedback.IsUserShare.Value)
                    {
                        var user = db.Users.AsNoTracking().FirstOrDefault(x => x.Id == userId);
                        feedback.UserFullName = user.Name;
                        feedback.UserMail = user.Email;
                        feedback.UserPhone = user.Phone;
                    }
                }
                catch (Exception)
                {

                }
                feedback.CreatedDate = DateTime.Now;
                feedback.UserId = userId;

                try
                {
                    feedback.IpAddress = this.Request.HttpContext.Connection
                        .RemoteIpAddress
                        .ToString();
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "IP Address error", Array.Empty<object>());
                }
                db.Feedback2.Add(feedback);
                db.SaveChanges();
                try
                {
                    var listMail = new List<string>();

                    var userPlaces = db.PanelUser.AsNoTracking().Where(x => x.PlaceId == feedback.PlaceId && !x.IsDeleted).ToList();
                    if (userPlaces != null && userPlaces.Any())
                    {
                        listMail.AddRange(userPlaces.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    var place = db.Places.First(x => x.Id == feedback.PlaceId);

                    var userCompanies = db.PanelUser.AsNoTracking().Where(x => x.CompanyId == place.CompanyId && !x.IsDeleted).ToList();
                    if (userCompanies != null && userCompanies.Any())
                    {
                        listMail.AddRange(userCompanies.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    _emailSender.Send(
                   listMail.ToArray()
                    , "Müşteriniz işletmeniz için bir değerlendirme yaptı", myTableMaker(new Feedback2[] { feedback }, "2"));
                }
                catch (Exception)
                {


                }
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
                var userId = 0;
                if (base.CurrentUser != null)
                {
                    userId = base.CurrentUser.UserId;
                }
                try
                {
                    if (feedback.IsUserShare.HasValue && feedback.IsUserShare.Value)
                    {
                        var user = db.Users.AsNoTracking().FirstOrDefault(x => x.Id == userId);
                        feedback.UserFullName = user.Name;
                        feedback.UserMail = user.Email;
                        feedback.UserPhone = user.Phone;
                    }
                }
                catch (Exception)
                {

                }
                feedback.CreatedDate = DateTime.Now;
                feedback.UserId = userId;

                try
                {
                    feedback.IpAddress = this.Request.HttpContext.Connection
                        .RemoteIpAddress
                        .ToString();
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "IP Address error", Array.Empty<object>());
                }
                db.Feedback3.Add(feedback);
                db.SaveChanges();

                try
                {
                    var listMail = new List<string>();

                    var userPlaces = db.PanelUser.AsNoTracking().Where(x => x.PlaceId == feedback.PlaceId && !x.IsDeleted).ToList();
                    if (userPlaces != null && userPlaces.Any())
                    {
                        listMail.AddRange(userPlaces.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    var place = db.Places.First(x => x.Id == feedback.PlaceId);

                    var userCompanies = db.PanelUser.AsNoTracking().Where(x => x.CompanyId == place.CompanyId && !x.IsDeleted).ToList();
                    if (userCompanies != null && userCompanies.Any())
                    {
                        listMail.AddRange(userCompanies.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    _emailSender.Send(
                   listMail.ToArray()
                    , "Müşteriniz işletmeniz için bir değerlendirme yaptı", myTableMaker(new Feedback3[] { feedback }, "3"));
                }
                catch (Exception)
                {


                }
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
                var userId = 0;
                if (base.CurrentUser != null)
                {
                    userId = base.CurrentUser.UserId;
                }
                try
                {
                    if (feedback.IsUserShare.HasValue && feedback.IsUserShare.Value)
                    {
                        var user = db.Users.AsNoTracking().FirstOrDefault(x => x.Id == userId);
                        feedback.UserFullName = user.Name;
                        feedback.UserMail = user.Email;
                        feedback.UserPhone = user.Phone;
                    }
                }
                catch (Exception)
                {

                }
                feedback.CreatedDate = DateTime.Now;
                feedback.UserId = userId;

                try
                {
                    feedback.IpAddress = this.Request.HttpContext.Connection
                        .RemoteIpAddress
                        .ToString();
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "IP Address error", Array.Empty<object>());
                }
                db.Feedback4.Add(feedback);
                db.SaveChanges();

                try
                {
                    var listMail = new List<string>();

                    var userPlaces = db.PanelUser.AsNoTracking().Where(x => x.PlaceId == feedback.PlaceId && !x.IsDeleted).ToList();
                    if (userPlaces != null && userPlaces.Any())
                    {
                        listMail.AddRange(userPlaces.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    var place = db.Places.First(x => x.Id == feedback.PlaceId);

                    var userCompanies = db.PanelUser.AsNoTracking().Where(x => x.CompanyId == place.CompanyId && !x.IsDeleted).ToList();
                    if (userCompanies != null && userCompanies.Any())
                    {
                        listMail.AddRange(userCompanies.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    _emailSender.Send(
                   listMail.ToArray()
                    , "Müşteriniz işletmeniz için bir değerlendirme yaptı", myTableMaker(new Feedback4[] { feedback }, "4"));
                }
                catch (Exception)
                {


                }
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
                var userId = 0;
                if (base.CurrentUser != null)
                {
                    userId = base.CurrentUser.UserId;
                }
                try
                {
                    if (feedback.IsUserShare.HasValue && feedback.IsUserShare.Value)
                    {
                        var user = db.Users.AsNoTracking().FirstOrDefault(x => x.Id == userId);
                        feedback.UserFullName = user.Name;
                        feedback.UserMail = user.Email;
                        feedback.UserPhone = user.Phone;
                    }
                }
                catch (Exception)
                {

                }
                feedback.CreatedDate = DateTime.Now;
                feedback.UserId = userId;

                try
                {
                    feedback.IpAddress = this.Request.HttpContext.Connection
                        .RemoteIpAddress
                        .ToString();
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "IP Address error", Array.Empty<object>());
                }
                db.Feedback5.Add(feedback);
                db.SaveChanges();
                try
                {
                    var listMail = new List<string>();

                    var userPlaces = db.PanelUser.AsNoTracking().Where(x => x.PlaceId == feedback.PlaceId && !x.IsDeleted).ToList();
                    if (userPlaces != null && userPlaces.Any())
                    {
                        listMail.AddRange(userPlaces.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    var place = db.Places.First(x => x.Id == feedback.PlaceId);

                    var userCompanies = db.PanelUser.AsNoTracking().Where(x => x.CompanyId == place.CompanyId && !x.IsDeleted).ToList();
                    if (userCompanies != null && userCompanies.Any())
                    {
                        listMail.AddRange(userCompanies.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    _emailSender.Send(
                   listMail.ToArray()
                    , "Müşteriniz işletmeniz için bir değerlendirme yaptı", myTableMaker(new Feedback5[] { feedback }, "5"));
                }
                catch (Exception)
                {


                }
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
                var userId = 0;
                if (base.CurrentUser != null)
                {
                    userId = base.CurrentUser.UserId;
                }
                try
                {
                    if (feedback.IsUserShare.HasValue && feedback.IsUserShare.Value)
                    {
                        var user = db.Users.AsNoTracking().FirstOrDefault(x => x.Id == userId);
                        feedback.UserFullName = user.Name;
                        feedback.UserMail = user.Email;
                        feedback.UserPhone = user.Phone;
                    }
                }
                catch (Exception)
                {

                }
                feedback.CreatedDate = DateTime.Now;
                feedback.UserId = userId;

                try
                {
                    feedback.IpAddress = this.Request.HttpContext.Connection
                        .RemoteIpAddress
                        .ToString();
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "IP Address error", Array.Empty<object>());
                }
                db.Feedback6.Add(feedback);
                db.SaveChanges();

                try
                {
                    var listMail = new List<string>();

                    var userPlaces = db.PanelUser.AsNoTracking().Where(x => x.PlaceId == feedback.PlaceId && !x.IsDeleted).ToList();
                    if (userPlaces != null && userPlaces.Any())
                    {
                        listMail.AddRange(userPlaces.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    var place = db.Places.First(x => x.Id == feedback.PlaceId);

                    var userCompanies = db.PanelUser.AsNoTracking().Where(x => x.CompanyId == place.CompanyId && !x.IsDeleted).ToList();
                    if (userCompanies != null && userCompanies.Any())
                    {
                        listMail.AddRange(userCompanies.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    _emailSender.Send(
                   listMail.ToArray()
                    , "Müşteriniz işletmeniz için bir değerlendirme yaptı", myTableMaker(new Feedback6[] { feedback }, "6"));
                }
                catch (Exception)
                {


                }
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
                var userId = 0;
                if (base.CurrentUser != null)
                {
                    userId = base.CurrentUser.UserId;
                }
                try
                {
                    if (feedback.IsUserShare.HasValue && feedback.IsUserShare.Value)
                    {
                        var user = db.Users.AsNoTracking().FirstOrDefault(x => x.Id == userId);
                        feedback.UserFullName = user.Name;
                        feedback.UserMail = user.Email;
                        feedback.UserPhone = user.Phone;
                    }
                }
                catch (Exception)
                {

                }
                feedback.CreatedDate = DateTime.Now;
                feedback.UserId = userId;
                try
                {
                    feedback.IpAddress = this.Request.HttpContext.Connection
                        .RemoteIpAddress
                        .ToString();
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "IP Address error", Array.Empty<object>());
                }
                db.Feedback7.Add(feedback);
                db.SaveChanges();


                try
                {
                    var listMail = new List<string>();

                    var userPlaces = db.PanelUser.AsNoTracking().Where(x => x.PlaceId == feedback.PlaceId && !x.IsDeleted).ToList();
                    if (userPlaces != null && userPlaces.Any())
                    {
                        listMail.AddRange(userPlaces.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    var place = db.Places.First(x => x.Id == feedback.PlaceId);

                    var userCompanies = db.PanelUser.AsNoTracking().Where(x => x.CompanyId == place.CompanyId && !x.IsDeleted).ToList();
                    if (userCompanies != null && userCompanies.Any())
                    {
                        listMail.AddRange(userCompanies.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                    }

                    _emailSender.Send(
                   listMail.ToArray()
                    , "Müşteriniz işletmeniz için bir değerlendirme yaptı", myTableMaker(new Feedback7[] { feedback }, "7"));
                }
                catch (Exception)
                {


                }
            }

        }
        catch (Exception ex)
        {
            return Content(ex.ToString());
        }
        return new EmptyResult();
    }


}

