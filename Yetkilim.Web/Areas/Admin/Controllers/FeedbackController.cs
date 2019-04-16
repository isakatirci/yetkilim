// FeedbackController
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Enums;
using Yetkilim.Global.Model;
using Yetkilim.Web.Areas.Admin.Controllers;
using Yetkilim.Web.Areas.Admin.Models;
using Yetkilim.Web.Models.Ef;

namespace Yetkilim.Web.Areas.Admin.Controllers
{
    public class FeedbackController : AdminBaseController
    {
        private readonly IFeedbackService _feedbackService;

        private readonly ILogger<ManageController> _logger;

        private readonly IPanelUserService _panelUserService;

        private readonly IPlaceService _placeService;

        public FeedbackController(ILogger<ManageController> logger, IFeedbackService feedbackService, IPlaceService placeService, IPanelUserService panelUserService)
        {
            _logger = logger;
            _feedbackService = feedbackService;
            _placeService = placeService;
            _panelUserService = panelUserService;
        }


        //public async Task<ViewResult> Index()
        //{
        //    try
        //    {
        //        int? companyId = base.CurrentUser.CompanyId;
        //        int? placeId = base.CurrentUser.PlaceId;

        //        var company = _companyService.GetCompanyAsync(CurrentUser.CompanyId).Result.Data;

        //        if (string.Equals(company.Demo, "Evet", StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            ViewBag.WarningDemo = "Dikkat bu işletme üyemiz değildir. Yine de değerlendirmenizi sorumlulara iletmeye çalışacağız. Dönüş geç yapılabilir ya da yapılmayabilir.";
        //        }


        //        using (yetkilimDBContext db = new yetkilimDBContext())
        //        {
        //            var companyFeedback = db.CompanyFeedback.First(x => x.TypeId == company.CompanyTypeId);
        //            return View("~/Views/Feedback/Feedback" + companyFeedback.FeedbackId + ".cshtml");
        //        }


        //    }
        //    catch (System.Exception ex)
        //    {


        //    }

        //}

        private bool myEquals(string value, string other)
        {
            return string.Equals(value, other, StringComparison.InvariantCultureIgnoreCase);
        }

        
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
            if (list.Length == 0)
            {
                return "";
            }

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var propertiesList = new List<PropertyInfo>();

            var headers = Consts.Headers;
            var keys = headers.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
               var prop = properties.FirstOrDefault(x => string.Equals(keys[i], x.Name, StringComparison.InvariantCultureIgnoreCase));
                if (prop != null)
                {
                    propertiesList.Add(prop);
                }
            }
            properties = propertiesList.ToArray();

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

                var isUserShare = false;
                var anonimMi = false;
                try
                {
                    var isUserShareProp = properties.FirstOrDefault(x => string.Equals("IsUserShare", x.Name, StringComparison.InvariantCultureIgnoreCase));
                    if (isUserShareProp != null)
                    {

                        var temp = (bool?)isUserShareProp.GetValue(list[i], null);
                        isUserShare = temp.HasValue && temp.Value;
                    }
                }
                catch (Exception)
                {


                }
                try
                {
                    var userIdProp = properties.FirstOrDefault(x => string.Equals("UserId", x.Name, StringComparison.InvariantCultureIgnoreCase));
                    if (userIdProp != null)
                    {
                        var temp = (int?)(userIdProp.GetValue(list[i], null));
                        anonimMi = temp.HasValue && temp < 1;
                    }
                }
                catch (Exception)
                {

                }


                table += "<tr>";
                for (int j = 0; j < properties.Length; j++)
                {
                    if (string.IsNullOrWhiteSpace(headers[properties[j].Name]))
                    {
                        continue;
                    }

                    if (string.Equals(properties[j].Name, "UserPhone", StringComparison.InvariantCultureIgnoreCase))
                    {
                        table += "<td>" + (anonimMi ? "Anonim" : (isUserShare ? properties[j].GetValue(list[i], null) : "Gizli")) + "</td>";
                        continue;
                    }

                    if (string.Equals(properties[j].Name, "UserFullName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        table += "<td>" + (anonimMi ? "Anonim" : (isUserShare ? properties[j].GetValue(list[i], null) : "Gizli")) + "</td>";
                        continue;
                    }

                    if (string.Equals(properties[j].Name, "UserMail", StringComparison.InvariantCultureIgnoreCase))
                    {
                        table += "<td>" + (anonimMi ? "Anonim" : (isUserShare ? properties[j].GetValue(list[i], null) : "Gizli")) + "</td>";
                        continue;
                    }

                    if (string.Equals(properties[j].Name, "PlaceId", StringComparison.InvariantCultureIgnoreCase))
                    {
                        table += "<td>" + GetPlaceName(properties[j].GetValue(list[i], null)) + "</td>";
                        continue;
                    }

                    table += "<td>" + properties[j].GetValue(list[i], null) + "</td>";
                }
                table += "</tr>";
            }
            table += "</tbody></table>";

            return table;
        }



        public async Task<IActionResult> Index()
        {
            try
            {
                CompanyFeedbacksModel model = new CompanyFeedbacksModel();
                var table = string.Empty;
                var feedbackCount = 0;
                //var feedbackCount = 0;

                using (Yetkilim.Web.Models.Ef.yetkilimDBContext db = new Yetkilim.Web.Models.Ef.yetkilimDBContext())
                {
                    db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    db.ChangeTracker.AutoDetectChangesEnabled = false;

                    var placesIds = new List<int?>();

                    if (base.CurrentUser.Role == UserRole.Admin)
                    {
                        var currentUserCompanyId = base.CurrentUser.CompanyId;
                        placesIds = db.Places.Where(x => x.CompanyId == currentUserCompanyId).Select(x => (int?)x.Id).ToList();
                    }
                    else if (base.CurrentUser.Role == UserRole.Dealer)
                    {
                        var currentPlaceId = base.CurrentUser.PlaceId;
                        placesIds = db.Places.Where(x => x.Id == currentPlaceId).Select(x => (int?)x.Id).ToList();
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback0.AsNoTracking()
                            .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x=>x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "0");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback1.AsNoTracking()
                            .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "1");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback2.AsNoTracking()
                             .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "2");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback3.AsNoTracking()
                             .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "3");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback4.AsNoTracking()
                            .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "4");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback5.AsNoTracking()
                             .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "5");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback6.AsNoTracking()
                            .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "6");
                    }

                    {
                        table += "<br/>";
                        var temp = db.Feedback7.AsNoTracking()
                             .WhereIf(base.CurrentUser.Role != UserRole.SuperAdmin, x => placesIds.Contains(x.PlaceId)).ToArray().OrderByDescending(x => x.CreatedDate ?? DateTime.Now).ToArray();
                        feedbackCount += temp.Length;
                        table += myTableMaker(temp, "7");
                    }
                    //model.PlaceCount = base.CurrentUser.Role != UserRole.SuperAdmin ? placesIds.Count : db.Places.Count();
                    //model.FeedbackCount = feedbackCount;

                }

                ViewBag.Table = table;
                ((dynamic)ViewBag).CompanyName = base.CurrentUser.CompanyName;
                return View((object)model);
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "Panel Index Error", Array.Empty<object>());
                return this.RedirectToAction("SignOut");
            }
        }


        public async Task<IActionResult> DataTable()
        {
            return View();
        }
    }
}

