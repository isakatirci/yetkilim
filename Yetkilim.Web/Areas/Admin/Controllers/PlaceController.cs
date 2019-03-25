using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Entity;
using Yetkilim.Domain.Enums;
using Yetkilim.Global.Model;
using Yetkilim.Web.Areas.Admin.Models;

namespace Yetkilim.Web.Areas.Admin.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Yetkilim.Business.Services;
    using Yetkilim.Domain.DTO;
    using Yetkilim.Domain.Entity;
    using Yetkilim.Domain.Enums;
    using Yetkilim.Global.Model;
    using Yetkilim.Web.Areas.Admin.Controllers;
    using Yetkilim.Web.Areas.Admin.Models;

    [Authorize(AuthenticationSchemes = "AdminAreaCookies", Roles = "SuperAdmin,Admin")]
    public class PlaceController : AdminBaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ILogger<PlaceController> _logger;

        private readonly IPlaceService _placeService;

        public PlaceController(ILogger<PlaceController> logger, IHostingEnvironment hostingEnvironment, IPlaceService placeService)
        {
            _logger = logger;
            _placeService = placeService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult LoadPlaceData()
        {
            //IL_0016: Unknown result type (might be due to invalid IL or missing references)
            //IL_001b: Unknown result type (might be due to invalid IL or missing references)
            //IL_002c: Unknown result type (might be due to invalid IL or missing references)
            //IL_004c: Unknown result type (might be due to invalid IL or missing references)
            //IL_006c: Unknown result type (might be due to invalid IL or missing references)
            //IL_009d: Unknown result type (might be due to invalid IL or missing references)
            //IL_00b6: Unknown result type (might be due to invalid IL or missing references)
            //IL_00d7: Unknown result type (might be due to invalid IL or missing references)
            //IL_00f9: Unknown result type (might be due to invalid IL or missing references)
            try
            {
                StringValues val = this.Request.Form["draw"];
                string draw = ((IEnumerable<string>)(object)this.Request.Form["draw"]).FirstOrDefault();
                string text = ((IEnumerable<string>)(object)this.Request.Form["start"]).FirstOrDefault();
                string text2 = ((IEnumerable<string>)(object)this.Request.Form["length"]).FirstOrDefault();
                string text3 = ((IEnumerable<string>)(object)this.Request.Form["columns[" + ((IEnumerable<string>)(object)this.Request.Form["order[0][column]"]).FirstOrDefault() + "][name]"]).FirstOrDefault();
                string text4 = ((IEnumerable<string>)(object)this.Request.Form["order[0][dir]"]).FirstOrDefault();
                string searchValue = ((IEnumerable<string>)(object)this.Request.Form["search[value]"]).FirstOrDefault();
                int count = (text2 != null) ? Convert.ToInt32(text2) : 0;
                int count2 = (text != null) ? Convert.ToInt32(text) : 0;
                int num = 0;
                IQueryable<Place> queryable = _placeService.GetPlaceQueryable();
                if (base.CurrentUser.Role != UserRole.SuperAdmin)
                {
                    int companyId = base.CurrentUser.CompanyId;
                    queryable = from w in queryable
                                where w.CompanyId == companyId
                                select w;
                }
                if (!string.IsNullOrEmpty(text3) || !string.IsNullOrEmpty(text4))
                {
                    queryable = DynamicQueryableExtensions.OrderBy<Place>(queryable, text3 + " " + text4, Array.Empty<object>());
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    queryable = from m in queryable
                                where m.Name.Contains(searchValue)
                                select m;
                }
                num = queryable.Count();
                List<Place> data = queryable.Skip(count2).Take(count).ToList();
                return this.Json((object)new
                {
                    draw = draw,
                    recordsFiltered = num,
                    recordsTotal = num,
                    data = data
                });
            }
            catch (Exception)
            {
                int num2 = 5;
                throw;
            }
        }

        public IActionResult Create()
        {
            PlaceFormModel placeFormModel = new PlaceFormModel
            {
                IsSuperAdmin = (base.CurrentUser.Role == UserRole.SuperAdmin)
            };
            return this.View((object)placeFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlaceFormModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    PlaceDTO placeDTO = Mapper.Map<PlaceFormModel, PlaceDTO>(model);
                    if (base.CurrentUser.Role != UserRole.SuperAdmin)
                    {
                        placeDTO.CompanyId = base.CurrentUser.CompanyId;
                    }
                    Result<Place> result = await _placeService.AddPlaceAsync(placeDTO);
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
            return this.View((object)model);
        }

        public async Task<ViewResult> Update(int id)
        {
            PlaceFormModel model = new PlaceFormModel
            {
                IsSuperAdmin = (base.CurrentUser.Role == UserRole.SuperAdmin)
            };
            try
            {
                Result<PlaceDTO> result = await _placeService.GetPlaceAsync(id);
                if (result.IsSuccess)
                {
                    model = Mapper.Map<PlaceDTO, PlaceFormModel>(result.Data);
                }
                else
                {
                    model.FormMessage = result.FormMessage;
                }
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
        public async Task<ViewResult> Update(int id, PlaceFormModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    PlaceDTO placeDTO = Mapper.Map<PlaceFormModel, PlaceDTO>(model);
                    placeDTO.CompanyId = ((base.CurrentUser.Role != UserRole.SuperAdmin) ? base.CurrentUser.CompanyId : 0);
                    Result result = await _placeService.UpdatePlaceAsync(id, placeDTO);
                    model.FormMessage = result.FormMessage;
                    model.IsSuccess = result.IsSuccess;
                    if (model.IsSuccess)
                    {
                        model.FormMessage = "İşleminiz başarılı bir şekilde gerçekleştirildi.";
                    }
                    model.IsSuperAdmin = (base.CurrentUser.Role == UserRole.SuperAdmin);
                    return this.View((object)model);
                }
                catch (Exception ex)
                {
                    LoggerExtensions.LogError(_logger, ex, "POST Update Error {0}", new object[1]
                    {
                    id
                    });
                    model.FormMessage = "İşleminiz gerçekleştirilemedi.";
                    model.IsSuperAdmin = (base.CurrentUser.Role == UserRole.SuperAdmin);
                    return this.View((object)model);
                }
            }
            model.IsSuperAdmin = (base.CurrentUser.Role == UserRole.SuperAdmin);
            return this.View((object)model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int? num = null;
                if (base.CurrentUser.Role != UserRole.SuperAdmin)
                {
                    num = base.CurrentUser.CompanyId;
                }
                Result result = await _placeService.DeletePlaceAsync(num, id);
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