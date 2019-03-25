// FeedbackController
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Enums;
using Yetkilim.Global.Model;
using Yetkilim.Web.Areas.Admin.Controllers;
using Yetkilim.Web.Areas.Admin.Models;

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

