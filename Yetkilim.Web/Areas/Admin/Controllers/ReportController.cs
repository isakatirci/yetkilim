using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Entity;
using Yetkilim.Domain.Enums;
using Yetkilim.Global.Model;
using Yetkilim.Infrastructure.Data.UnitOfWork;
using Yetkilim.Web.Areas.Admin.Models;
using Yetkilim.Web.Helpers;

namespace Yetkilim.Web.Areas.Admin.Controllers
{
    public class ReportController : AdminBaseController
    {
        private readonly IYetkilimUnitOfWork _unitOfWork;

        private readonly IPlaceService _placeService;

        public ReportController(IYetkilimUnitOfWork unitOfWork, IPlaceService placeService)
        {
            _unitOfWork = unitOfWork;
            _placeService = placeService;
        }

        public IActionResult Index(int p)
        {
            Report1Model report1Model = new Report1Model
            {
                PlaceId = p
            };
            Result<List<PlaceDTO>> result = _placeService.GetAllPlaceAsync(new PlaceSearchModel
            {
                CompanyId = base.CurrentUser.CompanyId,
                Page = 0,
                PageSize = 1000
            }).Result;
            if (p == 0 && base.CurrentUser.Role == UserRole.Dealer)
            {
                p = (base.CurrentUser.PlaceId ?? 0);
            }
            if (result.IsSuccess)
            {
                List<PlaceDTO> data = result.Data;
                if (p > 0)
                {
                    report1Model.ReportName = data.FirstOrDefault((PlaceDTO w) => w.Id == p)?.Name;
                }
                else
                {
                    report1Model.ReportName = base.CurrentUser.CompanyName;
                }
                if (base.CurrentUser.Role == UserRole.SuperAdmin || base.CurrentUser.Role == UserRole.Admin)
                {
                    report1Model.Places = result.Data;
                }
            }
            DateTime dateTime = DateTime.UtcNow.Date.AddMonths(-3);
            DateTime date = DateTime.UtcNow.Date;
            Random random = new Random();
            List<Feedback> list = new List<Feedback>();
            for (int i = 0; i < 300; i++)
            {
                Feedback item2 = new Feedback
                {
                    CreatedDate = dateTime.AddDays((double)random.Next(0, 90)),
                    Detail = new FeedbackDetail
                    {
                        AdviseRate = (int)(random.NextDouble() * 5.0),
                        FlavorRate = (int)(random.NextDouble() * 5.0),
                        CleaningRate = (int)(random.NextDouble() * 5.0),
                        PriceRate = (int)(random.NextDouble() * 5.0),
                        EmployeeRate = (int)(random.NextDouble() * 5.0)
                    }
                };
                list.Add(item2);
            }
            list = (from o in list
                    orderby o.CreatedDate
                    select o).ToList();
            List<LineReportModel> model = (from item in list
                                           group item by DateHelper.GetStartOfWeek(item.CreatedDate) into s
                                           select new LineReportModel
                                           {
                                               Key = DateHelper.ToUnixTimestamp(s.Key),
                                               Value = s.Count()
                                           }).ToList();
            List<LineReportModel> model2 = (from item in list
                                            group item by DateHelper.GetStartOfWeek(item.CreatedDate) into s
                                            select new LineReportModel
                                            {
                                                Key = DateHelper.ToUnixTimestamp(s.Key),
                                                Value = s.Average((Feedback x) => x.Detail.AdviseRate).ToString(CultureInfo.InvariantCulture)
                                            }).ToList();
            report1Model.Model1 = model;
            report1Model.Model2 = model2;
            List<LineReportModel> hijyenModel = (from item in list
                                                 group item by DateHelper.GetStartOfWeek(item.CreatedDate) into s
                                                 select new LineReportModel
                                                 {
                                                     Key = DateHelper.ToUnixTimestamp(s.Key),
                                                     Value = s.Average((Feedback x) => x.Detail.CleaningRate).ToString(CultureInfo.InvariantCulture)
                                                 }).ToList();
            List<LineReportModel> lezzetModel = (from item in list
                                                 group item by DateHelper.GetStartOfWeek(item.CreatedDate) into s
                                                 select new LineReportModel
                                                 {
                                                     Key = DateHelper.ToUnixTimestamp(s.Key),
                                                     Value = s.Average((Feedback x) => x.Detail.FlavorRate).ToString(CultureInfo.InvariantCulture)
                                                 }).ToList();
            List<LineReportModel> fiyatModel = (from item in list
                                                group item by DateHelper.GetStartOfWeek(item.CreatedDate) into s
                                                select new LineReportModel
                                                {
                                                    Key = DateHelper.ToUnixTimestamp(s.Key),
                                                    Value = s.Average((Feedback x) => x.Detail.PriceRate).ToString(CultureInfo.InvariantCulture)
                                                }).ToList();
            List<LineReportModel> ılgiModel = (from item in list
                                               group item by DateHelper.GetStartOfWeek(item.CreatedDate) into s
                                               select new LineReportModel
                                               {
                                                   Key = DateHelper.ToUnixTimestamp(s.Key),
                                                   Value = s.Average((Feedback x) => x.Detail.EmployeeRate).ToString(CultureInfo.InvariantCulture)
                                               }).ToList();
            report1Model.HijyenModel = hijyenModel;
            report1Model.LezzetModel = lezzetModel;
            report1Model.FiyatModel = fiyatModel;
            report1Model.IlgiModel = ılgiModel;
            return this.View((object)report1Model);
        }

        public IActionResult Index2()
        {
            Report2Model report2Model = new Report2Model();
            DateTime dateTime = DateTime.UtcNow.Date.AddMonths(-3);
            DateTime date = DateTime.UtcNow.Date;
            Random random = new Random();
            report2Model.RakiplerModelList = new List<OtherCompanyLineReportModel>();
            List<OtherCompanyLineReportModel> rakiplerModelList = report2Model.RakiplerModelList;
            for (int i = 0; i < 11; i++)
            {
                List<Feedback> list = new List<Feedback>();
                for (int j = 0; j < 300; j++)
                {
                    Feedback item2 = new Feedback
                    {
                        CreatedDate = dateTime.AddDays((double)random.Next(0, 90)),
                        Detail = new FeedbackDetail
                        {
                            AdviseRate = (int)(random.NextDouble() * 5.0),
                            FlavorRate = (int)(random.NextDouble() * 5.0),
                            CleaningRate = (int)(random.NextDouble() * 5.0),
                            PriceRate = (int)(random.NextDouble() * 5.0),
                            EmployeeRate = (int)(random.NextDouble() * 5.0)
                        }
                    };
                    list.Add(item2);
                }
                list = (from o in list
                        orderby o.CreatedDate
                        select o).ToList();
                List<LineReportModel> lineReportModel = (from item in list
                                                         group item by DateHelper.GetStartOfWeek(item.CreatedDate) into s
                                                         select new LineReportModel
                                                         {
                                                             Key = DateHelper.ToUnixTimestamp(s.Key),
                                                             Value = s.Average((Feedback x) => x.Detail.AdviseRate).ToString(CultureInfo.InvariantCulture)
                                                         }).ToList();
                string companyName = $"Firma{i}";
                if (i == 0)
                {
                    companyName = base.CurrentUser.CompanyName;
                }
                rakiplerModelList.Add(new OtherCompanyLineReportModel
                {
                    CompanyName = companyName,
                    LineReportModel = lineReportModel
                });
            }
            return this.View((object)report2Model);
        }
    }

}