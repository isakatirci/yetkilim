using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Web.Areas.Admin.Models;
using Yetkilim.Web.Helpers;

namespace Yetkilim.Web.Areas.Admin.Controllers
{
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
            return View();
        }

        [HttpPost]
        public IActionResult LoadPlaceData()
        {
            try
            {
                var x = Request.Form["draw"];
                var draw = Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();

                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();

                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;

                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                // getting all Customer data  
                var customerData = _placeService.GetPlaceQueryable();
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.Name.Contains(searchValue));
                }

                //total number of rows counts   
                recordsTotal = customerData.Count();
                //Paging   
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                var X = 5;
                throw;
            }

        }

        public IActionResult Create()
        {
            var model = new PlaceFormModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlaceFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {

                var place = Mapper.Map<PlaceFormModel, PlaceDTO>(model);

                var result = await _placeService.AddPlaceAsync(place);

                model.FormMessage = result.FormMessage;
                model.IsSuccess = result.IsSuccess;

                if (model.IsSuccess)
                    model.FormMessage = "İşleminiz başarılı bir şekilde gerçekleştirildi.";

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Error");
                model.IsSuccess = false;
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";

                return View(model);
            }
        }

        public async Task<ViewResult> Update(int id)
        {
            var model = new PlaceFormModel();
            try
            {
                var result = await _placeService.GetPlaceAsync(id);
                if (result.IsSuccess)
                    model = Mapper.Map<PlaceDTO, PlaceFormModel>(result.Data);
                else
                    model.FormMessage = result.FormMessage;

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET Update Error {0}", id);
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";

                return View(model);
            }
        }

        [HttpPost]
        public async Task<ViewResult> Update(int id, PlaceFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var place = Mapper.Map<PlaceFormModel, PlaceDTO>(model);

                var result = await _placeService.UpdatePlaceAsync(id, place);

                model.FormMessage = result.FormMessage;
                model.IsSuccess = result.IsSuccess;
                if (model.IsSuccess)
                    model.FormMessage = "İşleminiz başarılı bir şekilde gerçekleştirildi.";

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST Update Error {0}", id);
                model.FormMessage = "İşleminiz gerçekleştirilemedi.";

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _placeService.DeletePlaceAsync(id);

                if (result.IsSuccess)
                    return Json(new { result.IsSuccess });

                return Json(new { result.IsSuccess, result.FormMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST Delete Error {0}", id);

                return Json(new { IsSuccess = false, FormMessage = "İşleminiz gerçekleştirilemedi" });
            }
        }
    }
}