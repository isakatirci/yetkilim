using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
using Yetkilim.Global.Helpers;
using Yetkilim.Global.Model;
using Yetkilim.Infrastructure.Email;
using Yetkilim.Web.Areas.Admin.Controllers;
using Yetkilim.Web.Areas.Admin.Models;
using Yetkilim.Web.Models.Ef;

namespace Yetkilim.Web.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminAreaCookies", Roles = "SuperAdmin,Admin")]
    public class PlaceController : AdminBaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ILogger<PlaceController> _logger;

        private readonly IPlaceService _placeService;

        private readonly IEmailSender _emailSender;


        public PlaceController(ILogger<PlaceController> logger, IHostingEnvironment hostingEnvironment, IPlaceService placeService, IEmailSender emailSender)
        {
            _logger = logger;
            _placeService = placeService;
            _hostingEnvironment = hostingEnvironment;
            _emailSender = emailSender;
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
                var items = queryable.Skip(count2).Take(count).ToList();


                //                { "data": "id", "name": "Id", "autoWidth": true },
                //                { "data": "companyName", "name": "CompanyName", "autoWidth": true },
                //                { "data": "name", "name": "Name", "autoWidth": true },
                //                { "data": "address", "name": "Address", "autoWidth": true },
                //                { "data": "latitude", "name": "Latitude", "autoWidth": true },
                //                { "data": "longitude", "name": "Longitude", "autoWidth": true },
                //                { "data": "guest", "name": "Guest", "autoWidth": true },
                //                { "data": "createdDate", "name": "CreatedDate", "autoWidth": true },

                var data = items.Select(a => new {
                    a.Id,
                    CompanyName = a.Company.Name,
                    a.Name,
                    a.Address,
                    a.Latitude,
                    a.Longitude,
                    a.CreatedDate

             ,
                    Guest = string.Equals(a.Guest, "Evet", StringComparison.InvariantCultureIgnoreCase) ? "Evet" : "Hayır"
             ,

                }).ToList();


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


                    if (result.Messages != null && result.Messages.Any())
                    {
                        var temp = result.Messages.FirstOrDefault(x => string.Equals(x, "Demo", StringComparison.InvariantCultureIgnoreCase));
                        if (temp != null)
                        {
                            using (yetkilimDBContext db = new yetkilimDBContext())
                            {
                                var listMail = new List<string>();

                                //var userPlaces = db.PanelUser.AsNoTracking().Where(x => x.PlaceId == feedback.PlaceId).ToList();
                                //if (userPlaces != null && userPlaces.Any())
                                //{
                                //    listMail.AddRange(userPlaces.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                                //}

                                //var place = db.Places.First(x => x.Id == feedback.PlaceId);

                                var placeUsers = db.PanelUser.Where(x => x.PlaceId == id && !x.IsDeleted).ToList();

                                for (int i = 0; i < placeUsers.Count; i++)
                                {
                                    try
                                    {
                                        string pass = PasswordHelper.GeneratePassword(6);
                                        placeUsers[i].Password = PasswordHelper.MD5Hash(pass);
                                        db.Entry(placeUsers[i]).State = EntityState.Modified;
                                        db.SaveChanges();
                                        await _emailSender.Send(new string[] { placeUsers[i].Email }, "Yeni şireniz", pass);
                                    }
                                    catch (Exception ex)
                                    {


                                    }

                                }


                                // if (userCompanies != null && userCompanies.Any())
                                // {
                                //     listMail.AddRange(userCompanies.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email));
                                // }

                                // foreach (var item in listMail)
                                // {
                                //     ;
                                // }                                
                            }
                        }
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