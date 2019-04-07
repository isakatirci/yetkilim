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

        private string myTableMaker<T>(T[] list)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //Array.Sort(properties, new ComparerPropertyInfo());
            var table = "<table class=\"table\" id=\"sample-data-table-feedback\"><thead><tr>";
            for (int j = 0; j < properties.Length; j++)
            {
                table += "<th class=\"secondary-text\"><div class=\"table-header\"><span class=\"column-title\">" + properties[j].Name + "</span></div></td>";
            }
            table += "</tr></thead><tbody>";
            for (int i = 0; i < list.Length; i++)
            {
                table += "<tr>";
                for (int j = 0; j < properties.Length; j++)
                {
                    table += "<td>" + properties[j].GetValue(list[i], null) + "</td>";
                }
                table += "</tr>";
            }
            table += "</tbody></table>";

            return table;
        }

        public IActionResult FeedbackIndex(string feedbackid)
        {
            if (string.IsNullOrWhiteSpace(feedbackid))
            {
                return RedirectToAction(actionName: "Index");
            }

            var table = string.Empty;

            using (yetkilimDBContext db = new yetkilimDBContext())
            {
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                db.ChangeTracker.AutoDetectChangesEnabled = false;
                //db.ProxyCreationEnabled = false;            
                if (myEquals(feedbackid, "0"))
                {
                    table = myTableMaker(db.Feedback0.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "1"))
                {
                    table = myTableMaker(db.Feedback1.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "2"))
                {
                    table = myTableMaker(db.Feedback2.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "3"))
                {
                    table = myTableMaker(db.Feedback3.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "4"))
                {
                    table = myTableMaker(db.Feedback4.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "5"))
                {
                    table = myTableMaker(db.Feedback5.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "6"))
                {
                    table = myTableMaker(db.Feedback6.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "7"))
                {
                    table = myTableMaker(db.Feedback7.AsNoTracking().ToArray());
                }
            }

            ViewBag.Table = table;

            return View();
        }


        public async Task<ViewResult> Index()
        {
            CompanyFeedbacksModel model = new CompanyFeedbacksModel();
            int? companyId = base.CurrentUser.CompanyId;
            int? placeId = base.CurrentUser.PlaceId;
            if (base.CurrentUser.Role == UserRole.SuperAdmin)
            {
                companyId = null;
            }
            CompanyUserSearchModel companyUserSearchModel = new CompanyUserSearchModel
            {
                CompanyId = companyId,
                PlaceId = placeId,
                Page = 1,
                PageSize = 1000
            };
            Result<List<FeedbackDetailDTO>> result = await _feedbackService.GetAllFeedbackDetailAsync(companyUserSearchModel);
            if (result.IsSuccess)
            {
                model.Feedbacks = (from o in result.Data
                                   orderby o.CreatedDate descending
                                   select o).ToList();
            }
            if (base.CurrentUser.Role == UserRole.SuperAdmin || base.CurrentUser.Role == UserRole.Admin)
            {
                Result<List<PlaceDTO>> result2 = _placeService.GetAllPlaceAsync(new PlaceSearchModel
                {
                    CompanyId = base.CurrentUser.CompanyId,
                    Page = 0,
                    PageSize = 1000
                }).Result;
                if (result2.IsSuccess)
                {
                    model.Places = result2.Data;
                }
            }
            return this.View((object)model);
        }
    }
}

