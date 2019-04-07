using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Entity;
using Yetkilim.Domain.Enums;
using Yetkilim.Global.Model;
using Yetkilim.Web.Areas.Admin.Models;
using Yetkilim.Web.Models.Ef;

namespace Yetkilim.Web.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminAreaCookies", Roles = "SuperAdmin,Admin")]
    public class UserController : AdminBaseController
    {
        private readonly ILogger<UserController> _logger;

        private readonly IPanelUserService _panelUserService;

        private readonly IPlaceService _placeService;

        public UserController(ILogger<UserController> logger, IPanelUserService panelUserService, IPlaceService placeService)
        {
            _logger = logger;
            _panelUserService = panelUserService;
            _placeService = placeService;
        }

        public async Task<ViewResult> Index()
        {
            PanelUserModel model = new PanelUserModel();
            int? companyId = base.CurrentUser.CompanyId;
            if (base.CurrentUser.Role == UserRole.SuperAdmin)
            {
                model.IsSuperAdmin = true;
                companyId = null;
            }
            PanelUserSearchModel panelUserSearchModel = new PanelUserSearchModel
            {
                CompanyId = companyId,
                Page = 1,
                PageSize = 1000
            };
            Result<List<PanelUserDTO>> result = await _panelUserService.GetAllUserAsync(panelUserSearchModel);
            if (result.IsSuccess)
            {
                model.Users = result.Data;
            }
            return this.View((object)model);
        }

        private void FillPanelUserFormModel(PanelUserFormModel model)
        {
            if (base.CurrentUser.UserId == model.UserId)
            {
                model.IsProfile = true;
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
        }

        public IActionResult Create()
        {
            PanelUserFormModel panelUserFormModel = new PanelUserFormModel
            {
                Role = UserRole.Dealer
            };
            FillPanelUserFormModel(panelUserFormModel);
            return this.View((object)panelUserFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PanelUserFormModel model)
        {
            try
            {
                FillPanelUserFormModel(model);
                if (!this.ModelState.IsValid)
                {
                    return this.View((object)model);
                }
                if (model.Role == UserRole.Dealer && !model.PlaceId.HasValue)
                {
                    model.IsSuccess = false;
                    model.FormMessage = "Şube yetkilisi için mekan seçmeniz gerekmektedir.";
                    return this.View((object)model);
                }

                var companyId = 0;

                //if (model.PlaceId.HasValue)
                //{
                //    try
                //    {
                //        using (yetkilimDBContext db = new yetkilimDBContext())
                //        {
                //            var place = db.Places.FirstOrDefault(x => x.Id == model.PlaceId);
                //            companyId = place.CompanyId;
                //        }
                      
                //    }
                //    catch (Exception ex)
                //    {

                        
                //    }                   
                //}
                //else
                //{                    
                //    companyId = base.CurrentUser.CompanyId;
                //}

                PanelUserDTO panelUserDTO = new PanelUserDTO
                {
                    Name = model.Name,
                    Email = model.Email,
                    Role = model.Role,
                    CreatedDate = DateTime.UtcNow,
                    CompanyId = companyId = base.CurrentUser.CompanyId,
                    PlaceId = model.PlaceId
                };
                if (model.Role == UserRole.Dealer && base.CurrentUser.Role == UserRole.SuperAdmin)
                {
                    Place place = _placeService.GetPlaceQueryable().FirstOrDefault((Place w) => (object)(int?)w.Id == (object)model.PlaceId);
                    if (place != null)
                    {
                        panelUserDTO.CompanyId = place.CompanyId;
                    }
                }
                Result<PanelUserDTO> result = await _panelUserService.AddUserAsync(panelUserDTO);
                model.FormMessage = result.FormMessage;
                model.IsSuccess = result.IsSuccess;
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

        public async Task<ViewResult> Update(int id)
        {
            PanelUserFormModel model = new PanelUserFormModel
            {
                UserId = id
            };
            try
            {
                Result<PanelUserDTO> result = await _panelUserService.GetUserAsync(id);
                if (result.IsSuccess)
                {
                    model = Mapper.Map<PanelUserDTO, PanelUserFormModel>(result.Data);
                }
                else
                {
                    model.FormMessage = result.FormMessage;
                }
                model.UserId = id;
                FillPanelUserFormModel(model);
                return this.View((object)model);
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "GET Update Error {0}", new object[1]
                {
                id
                });
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                return this.View((object)model);
            }
        }

        [HttpPost]
        public async Task<ViewResult> Update(int id, PanelUserFormModel model)
        {
            model.UserId = id;
            FillPanelUserFormModel(model);
            if (this.ModelState.IsValid)
            {
                try
                {
                    PanelUserDTO panelUserDTO = Mapper.Map<PanelUserFormModel, PanelUserDTO>(model);
                    if (model.Role == UserRole.Dealer && base.CurrentUser.Role == UserRole.SuperAdmin)
                    {
                        Place place = _placeService.GetPlaceQueryable().FirstOrDefault((Place w) => (object)(int?)w.Id == (object)model.PlaceId);
                        if (place != null)
                        {
                            panelUserDTO.CompanyId = place.CompanyId;
                        }
                    }
                    Result result = await _panelUserService.UpdateUserAsync(id, panelUserDTO);
                    model.FormMessage = result.FormMessage;
                    model.IsSuccess = result.IsSuccess;
                    if (model.IsSuccess)
                    {
                        model.FormMessage = "İşleminiz başarılı bir şekilde gerçekleştirildi.";
                    }
                    return this.View((object)model);
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "POST Update Error {0}", new object[1]
                    {
                    id
                    });
                    model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                    return this.View((object)model);
                }
            }
            return this.View((object)model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == base.CurrentUser.UserId)
                {
                    return this.Json((object)new
                    {
                        IsSuccess = false,
                        FormMessage = "İşleminiz gerçekleştirilemedi, kendi kullanıcınızı silemezsiniz."
                    });
                }
                Result result = await _panelUserService.DeleteUserAsync(id);
                if (result.IsSuccess)
                {
                    return this.Json((object)new
                    {
                        result.IsSuccess
                    });
                }
                return this.Json((object)new
                {
                    result.IsSuccess,
                    result.FormMessage
                });
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "POST Delete Error {0}", new object[1]
                {
                id
                });
                return this.Json((object)new
                {
                    IsSuccess = false,
                    FormMessage = "İşleminiz gerçekleştirilemedi"
                });
            }
        }
    }
}