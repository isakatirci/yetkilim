using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
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

    public class PromotionController : AdminBaseController
    {
        private readonly IPromotionService _promotionService;

        private readonly ILogger<PromotionController> _logger;

        private readonly IPlaceService _placeService;

        private readonly IFeedbackService _feedbackService;

        public PromotionController(ILogger<PromotionController> logger, IPromotionService promotionService, IPlaceService placeService, IFeedbackService feedbackService)
        {
            _promotionService = promotionService;
            _placeService = placeService;
            _feedbackService = feedbackService;
            _logger = logger;
        }

        public async Task<ViewResult> Index()
        {
            PromotionViewModel model = new PromotionViewModel();
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
            Result<List<PromotionDTO>> result = await _promotionService.GetAllPromotionAsync(companyUserSearchModel);
            if (result.IsSuccess)
            {
                model.Promotions = (from o in result.Data
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

        private void FillPromotionFormModel(PromotionFormModel model)
        {
            if (model.DueDate == DateTime.MinValue)
            {
                model.DueDate = DateTime.Today;
            }
            if (base.CurrentUser.Role == UserRole.SuperAdmin)
            {
                model.IsSuperAdmin = true;
            }
            int? companyId = base.CurrentUser.CompanyId;
            if (base.CurrentUser.Role == UserRole.SuperAdmin)
            {
                companyId = null;
            }
            Result<List<PlaceDTO>> result = _placeService.GetAllPlaceAsync(new PlaceSearchModel
            {
                CompanyId = companyId,
                Page = 0,
                PageSize = 1000
            }).Result;
            if (result.IsSuccess)
            {
                model.Places = result.Data;
            }
            CompanyUserSearchModel companyUserSearchModel = new CompanyUserSearchModel
            {
                CompanyId = companyId,
                PlaceId = base.CurrentUser.PlaceId,
                Page = 1,
                PageSize = 1000
            };
            Result<List<FeedbackDetailDTO>> result2 = _feedbackService.GetAllFeedbackDetailAsync(companyUserSearchModel).Result;
            if (result2.IsSuccess)
            {
                List<FeedbackDetailDTO> data = result2.Data;
                model.Users = (from o in EnumerableExtensions.Distinct<UserDTO>(from s in data.Where(delegate (FeedbackDetailDTO w)
                {
                    if (w.IsUserShare && !w.IsAnon)
                    {
                        return w.User.Email != null;
                    }
                    return false;
                })
                                                                                select s.User, (Func<UserDTO, UserDTO, bool>)((UserDTO dto, UserDTO userDTO) => dto.Id == userDTO.Id))
                               orderby o.CreatedDate descending
                               select o).ToList();
            }
        }

        public IActionResult Create()
        {
            PromotionFormModel promotionFormModel = new PromotionFormModel
            {
                IsActive = true
            };
            FillPromotionFormModel(promotionFormModel);
            return this.View((object)promotionFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PromotionFormModel model)
        {
            try
            {
                FillPromotionFormModel(model);
                if (!this.ModelState.IsValid)
                {
                    return this.View((object)model);
                }
                if (!DateTime.TryParseExact(model.DueDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    model.IsSuccess = false;
                    model.FormMessage = "Geçerli bir tarih giriniz.";
                    return this.View((object)model);
                }
                model.DueDate = result;
                if (model.DueDate < DateTime.Today)
                {
                    model.IsSuccess = false;
                    model.FormMessage = "Son kullanım tarihi bugünden önce olamaz.";
                    return this.View((object)model);
                }
                PromotionDTO promotionDTO = new PromotionDTO
                {
                    IsActive = true,
                    Message = model.Message,
                    DueDate = model.DueDate,
                    UserId = model.UserId,
                    PlaceId = model.PlaceId,
                    CreatedBy = base.CurrentUser.FullName
                };
                if (base.CurrentUser.Role == UserRole.Dealer)
                {
                    model.PlaceId = (base.CurrentUser.PlaceId ?? 0);
                }
                Result<PromotionDTO> result2 = await _promotionService.AddPromotionAsync(promotionDTO);
                model.FormMessage = result2.FormMessage;
                model.IsSuccess = result2.IsSuccess;
                if (model.IsSuccess)
                {
                    model.FormMessage = "İşleminiz başarılı bir şekilde gerçekleştirildi.";
                }
                return this.View((object)model);
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "Create Error", Array.Empty<object>());
                model.IsSuccess = false;
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return this.View((object)model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id)
        {
            PromotionFormModel model = new PromotionFormModel();
            try
            {
                Result result = await _promotionService.UsedPromotionAsync(id);
                model.FormMessage = result.FormMessage;
                model.IsSuccess = result.IsSuccess;
                if (model.IsSuccess)
                {
                    model.FormMessage = "İşleminiz başarılı bir şekilde gerçekleştirildi.";
                }
                return this.Json((object)model);
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "POST Update Error {0}", new object[1]
                {
                id
                });
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return this.Json((object)model);
            }
        }
    }

}