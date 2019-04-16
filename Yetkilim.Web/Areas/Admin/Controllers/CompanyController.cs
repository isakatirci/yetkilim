// CompanyController
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
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
using Yetkilim.Web.Helpers;
using Yetkilim.Web.Models;
using Yetkilim.Web.Models.Ef;

namespace Yetkilim.Web.Areas.Admin.Controllers
{


    public class CompanyController : AdminBaseController
    {
        private readonly ICompanyService _companyService;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IEmailSender _emailSender;

        private readonly ILogger<CompanyController> _logger;

        Dictionary<int, string> _companyTypes = new Dictionary<int, string>() {
           {1,"Cafe" },
           {2, "Restoran"},
           {3, "Otel"},
           {4, "Güzellik Salonu"},
           {5, "Benzin İstasyonu"},
           {6, "Kuaför"},
           {7, "Seyahat Şirketi"},
           {8, "Hastane"},
           {9, "Mağaza"},
           {10, "Market"},
           {11, "Event"}
        };

        public CompanyController(ILogger<CompanyController> logger, IHostingEnvironment hostingEnvironment, ICompanyService companyService, IEmailSender emailSender)
        {
            _logger = logger;
            _companyService = companyService;
            _hostingEnvironment = hostingEnvironment;
            _emailSender = emailSender;
        }

        [Authorize(AuthenticationSchemes = "AdminAreaCookies", Roles = "SuperAdmin")]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult LoadCompanyData()
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
                IQueryable<Company> queryable = _companyService.GetCompanyQueryable();
                if (!string.IsNullOrEmpty(text3) || !string.IsNullOrEmpty(text4))
                {
                    queryable = DynamicQueryableExtensions.OrderBy<Company>(queryable, text3 + " " + text4, Array.Empty<object>());
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    queryable = from m in queryable
                                where m.Name.Contains(searchValue)
                                select m;
                }
                num = queryable.Count();

                //List<Company> data = queryable.Skip(count2).Take(count).ToList();
                var items = queryable.Skip(count2).Take(count).ToList();

                var data = items.Select(a => new {
                    a.Id,
                    a.Name,
                    a.Address,
                    a.CreatedDate

                    ,
                    Demo = string.Equals(a.Demo, "Evet", StringComparison.InvariantCultureIgnoreCase) ? "Evet" : "Hayır"
                    ,
                    CompanyTypeName = _companyTypes.ContainsKey(a.CompanyTypeId) ? _companyTypes[a.CompanyTypeId] : "Diğer"

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

        [Authorize(AuthenticationSchemes = "AdminAreaCookies", Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            CompanyFormModel companyFormModel = new CompanyFormModel();
            return this.View((object)companyFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompanyFormModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(model.ManagerName) || string.IsNullOrWhiteSpace(model.ManagerEmail))
                    {
                        model.IsSuccess = false;
                        model.FormMessage = "Firma yetkilisi bilgileri girilmelidir.";
                        return this.View((object)model);
                    }
                    IFormFile logoFile = model.LogoFile;
                    string uniqueFileName = null;
                    if (logoFile != null && logoFile.Length > 0)
                    {
                        uniqueFileName = FileHelper.GetUniqueFileName(logoFile.FileName);
                        if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
                        {
                            _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        }
                        string path = Path.Combine(_hostingEnvironment.WebRootPath, "admin/uploads");
                        string path2 = Path.Combine(path, uniqueFileName);
                        using (FileStream stream = new FileStream(path2, FileMode.Create))
                        {
                            await logoFile.CopyToAsync((Stream)stream, default(CancellationToken));
                        }
                    }
                    CompanyDetailDTO companyDetailDTO = Mapper.Map<CompanyFormModel, CompanyDetailDTO>(model);
                    companyDetailDTO.Image = uniqueFileName;
                    PanelUserDTO manager = new PanelUserDTO
                    {
                        Email = model.ManagerEmail,
                        Name = model.ManagerName,
                        Surname = model.ManagerSurname
                    };
                    Result<CompanyDetailDTO> result = await _companyService.AddCompanyAsync(companyDetailDTO, manager);
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

        [Authorize(AuthenticationSchemes = "AdminAreaCookies", Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Update(int id)
        {
            if (base.CurrentUser.Role != UserRole.Dealer && (base.CurrentUser.Role != UserRole.Admin || base.CurrentUser.CompanyId == id))
            {
                CompanyFormModel model = new CompanyFormModel();
                try
                {
                    Result<CompanyDetailDTO> result = await _companyService.GetCompanyAsync(id);
                    if (result.IsSuccess)
                    {
                        model = Mapper.Map<CompanyDetailDTO, CompanyFormModel>(result.Data);
                    }
                    else
                    {
                        model.FormMessage = result.FormMessage;
                    }
                    model.IsUpdate = true;
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
            return this.RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "AdminAreaCookies", Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Update(int id, CompanyFormModel model)
        {
            if (base.CurrentUser.Role != UserRole.SuperAdmin && base.CurrentUser.CompanyId != id)
            {
                return this.RedirectToAction("Index", "Manage");
            }
            model.IsUpdate = true;
            if (this.ModelState.IsValid)
            {
                try
                {
                    CompanyDetailDTO company = Mapper.Map<CompanyFormModel, CompanyDetailDTO>(model);
                    IFormFile logoFile = model.LogoFile;
                    if (logoFile != null)
                    {
                        string uniqueFileName = FileHelper.GetUniqueFileName(logoFile.FileName);
                        if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
                        {
                            _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        }
                        string path = Path.Combine(_hostingEnvironment.WebRootPath, "admin/uploads");
                        string path2 = Path.Combine(path, uniqueFileName);
                        using (FileStream stream = new FileStream(path2, FileMode.Create))
                        {
                            await logoFile.CopyToAsync((Stream)stream, default(CancellationToken));
                        }
                        company.Image = uniqueFileName;
                        model.Image = uniqueFileName;
                    }
                    Result result = await _companyService.UpdateCompanyAsync(id, company);
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

                                var userCompanies = db.PanelUser.Where(x => x.CompanyId == id && !x.IsDeleted).ToList();

                                for (int i = 0; i < userCompanies.Count; i++)
                                {
                                    try
                                    {
                                        string pass = PasswordHelper.GeneratePassword(6);
                                        userCompanies[i].Password = PasswordHelper.MD5Hash(pass);
                                        db.Entry(userCompanies[i]).State = EntityState.Modified;
                                        db.SaveChanges();
                                        await _emailSender.Send(new string[] { userCompanies[i].Email }, "Yeni şireniz", pass);
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

        [Authorize(AuthenticationSchemes = "AdminAreaCookies", Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Result result = await _companyService.DeleteCompanyAsync(id);
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

        [HttpGet]
        public async Task<JsonResult> QuickSearchCompany(QuickSearchModel quickSearchModel)
        {
            try
            {
                return this.Json((object)new
                {
                    items = (await _companyService.GetAllCompanyAsync(quickSearchModel.ToSearchModel())).Data
                });
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, ex, "QuickSearchCompany {0}", new object[1]
                {
                quickSearchModel
                });
                return this.Json((object)Result.Fail("İşleminiz gerçekleştirilemedi!", ex));
            }
        }
    }
}