using System;
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
using Yetkilim.Global;
using Yetkilim.Global.Model;
using Yetkilim.Web.Areas.Admin.Models;
using Yetkilim.Web.Helpers;
using Yetkilim.Web.Models;

namespace Yetkilim.Web.Areas.Admin.Controllers
{
    public class CompanyController : AdminBaseController
    {
        private readonly ICompanyService _companyService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ILogger<CompanyController> logger, IHostingEnvironment hostingEnvironment,
            ICompanyService companyService)
        {
            _logger = logger;
            _companyService = companyService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadCompanyData()
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
                var sortColumn = Request
                    .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                var pageSize = length != null ? Convert.ToInt32(length) : 0;

                var skip = start != null ? Convert.ToInt32(start) : 0;

                var recordsTotal = 0;

                // getting all Customer data  
                var customerData = _companyService.GetCompanyQueryable();
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                    customerData = customerData.Where(m => m.Name.Contains(searchValue));

                //total number of rows counts   
                recordsTotal = customerData.Count();
                //Paging   
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new {draw, recordsFiltered = recordsTotal, recordsTotal, data});
            }
            catch (Exception ex)
            {
                var X = 5;
                throw;
            }
        }

        public IActionResult Create()
        {
            var model = new CompanyFormModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompanyFormModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var logoFile = model.LogoFile;
                string uniqueFileName = null;
                if (logoFile != null && logoFile.Length > 0)
                {
                    uniqueFileName = FileHelper.GetUniqueFileName(logoFile.FileName);
                    var uploadDirectory =
                        Path.Combine(_hostingEnvironment.WebRootPath, Consts.UploadFolders.AdminBaseFolder);
                    var filePath = Path.Combine(uploadDirectory, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await logoFile.CopyToAsync(stream);
                    }
                }

                var company = Mapper.Map<CompanyFormModel, CompanyDetailDTO>(model);
                company.Image = uniqueFileName;

                // TODO: şimdilik
                company.CompanyTypeId = 1;

                var managerUser = new PanelUserDTO()
                {
                    Email = model.ManagerEmail,
                    Name = model.ManagerName,
                    Surname = model.ManagerSurname
                };

                var result = await _companyService.AddCompanyAsync(company, managerUser);

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
            var model = new CompanyFormModel();
            try
            {
                var result = await _companyService.GetCompanyAsync(id);
                if (result.IsSuccess)
                    model = Mapper.Map<CompanyDetailDTO, CompanyFormModel>(result.Data);
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
        public async Task<ViewResult> Update(int id, CompanyFormModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var company = Mapper.Map<CompanyFormModel, CompanyDetailDTO>(model);
                // TODO: şimdilik
                company.CompanyTypeId = 1;

                var logoFile = model.LogoFile;
                if (logoFile != null)
                {
                    var uniqueFileName = FileHelper.GetUniqueFileName(logoFile.FileName);
                    var uploadDirectory = Path.Combine(_hostingEnvironment.WebRootPath,
                        Consts.UploadFolders.AdminBaseFolder);
                    var filePath = Path.Combine(uploadDirectory, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await logoFile.CopyToAsync(stream);
                    }

                    company.Image = uniqueFileName;
                    model.Image = uniqueFileName;
                }

                var result = await _companyService.UpdateCompanyAsync(id, company);

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
                var result = await _companyService.DeleteCompanyAsync(id);

                if (result.IsSuccess)
                    return Json(new {result.IsSuccess});

                return Json(new { result.IsSuccess , result.FormMessage});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST Delete Error {0}", id);

                return Json(new {IsSuccess = false, FormMessage = "İşleminiz gerçekleştirilemedi"});
            }
        }

        [HttpGet]
        public async Task<JsonResult> QuickSearchCompany(QuickSearchModel quickSearchModel)
        {
            try
            {
                var res = await _companyService.GetAllCompanyAsync(quickSearchModel.ToSearchModel());
                return Json(new {items = res.Data});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "QuickSearchCompany {0}", quickSearchModel);

                var res = Result.Fail("İşleminiz gerçekleştirilemedi!", ex);
                return Json(res);
            }
        }
    }
}