// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.CompanyService
// Assembly: Yetkilim.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 14D1D813-227D-40DF-9953-925BA95C932F
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Business.dll

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Entity;
using Yetkilim.Domain.Enums;
using Yetkilim.Global.Context;
using Yetkilim.Global.Model;
using Yetkilim.Infrastructure.Data.UnitOfWork;

namespace Yetkilim.Business.Services
{
    public class CompanyService : ServiceBase, ICompanyService
    {
        private readonly IPanelUserService _panelUserService;

        private readonly IYetkilimUnitOfWork _unitOfWork;

        public CompanyService(IPanelUserService panelUserService, IYetkilimUnitOfWork unitOfWork, IGlobalContext globalContext)
            : base(globalContext)
        {
            _panelUserService = panelUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CompanyDetailDTO>> AddCompanyAsync(CompanyDetailDTO company, PanelUserDTO manager)
        {
            Result<CompanyDetailDTO> res = new Result<CompanyDetailDTO>();
            _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                Company item = Mapper.Map<CompanyDetailDTO, Company>(company);
                Company created = await _unitOfWork.EntityRepository<Company>().CreateAsync(item);
                await _unitOfWork.SaveChangesAsync();
                manager.CompanyId = created.Id;
                manager.Role = UserRole.Admin;
                Result<PanelUserDTO> userRes = await _panelUserService.AddUserAsync(manager);
                if (!userRes.IsSuccess)
                {
                    _unitOfWork.Rollback();
                    return res.Fail(userRes.Messages);
                }
                _unitOfWork.Commit();
                CompanyDetailDTO mapItem = Mapper.Map<Company, CompanyDetailDTO>(created);
                return Result.Data(mapItem);
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public IQueryable<Company> GetCompanyQueryable()
        {
            return _unitOfWork.EntityRepository<Company>().GetQueryable(null, null);
        }

        public async Task<Result<List<CompanyInfoDTO>>> GetAllCompanyAsync(SearchModel searchModel)
        {
            searchModel.FixPageDefinations();
            IQueryable<Company> query = _unitOfWork.EntityRepository<Company>().GetQueryable(null, null);
            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                query = from w in query
                        where w.Name.Contains(searchModel.SearchText)
                        select w;
            }
            return Result.Data(await EntityFrameworkQueryableExtensions.ToListAsync<CompanyInfoDTO>(from s in (from o in query
                                                                                                               orderby o.Name
                                                                                                               select o).Skip(searchModel.PageSize * searchModel.PageIndex).Take(searchModel.PageSize)
                                                                                                    select new CompanyInfoDTO
                                                                                                    {
                                                                                                        Id = s.Id,
                                                                                                        Name = s.Name,
                                                                                                        Image = s.Image
                                                                                                    }, default(CancellationToken)));
        }

        public async Task<Result<CompanyDetailDTO>> GetCompanyAsync(int id)
        {
            CompanyDetailDTO res = Mapper.Map<Company, CompanyDetailDTO>(await _unitOfWork.EntityRepository<Company>().GetFirstAsync((Company w) => w.Id == id, null));
            return Result.Data(res);
        }

        public async Task<Result> UpdateCompanyAsync(int id, CompanyDetailDTO company)
        {
            Company item = await _unitOfWork.EntityRepository<Company>().GetFirstAsync((Company w) => w.Id == id, null);
            if (item == null)
            {
                return Result.Fail("Firma bulunamadı!");
            }
            item.Name = company.Name;
            item.Address = company.Address;
            item.CompanyTypeId = company.CompanyTypeId;
            if (!string.IsNullOrWhiteSpace(company.Image))
            {
                item.Image = company.Image;
            }
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteCompanyAsync(int id)
        {
            Company item = await _unitOfWork.EntityRepository<Company>().GetFirstAsync((Company w) => w.Id == id, null);
            if (item == null)
            {
                return Result.Fail("Firma bulunamadı!");
            }
            if (await _unitOfWork.EntityRepository<Place>().GetCountAsync((Place w) => w.CompanyId == id) > 0)
            {
                return Result.Fail("Firmaya ait mekan olduğu için silinmedi!");
            }
            _unitOfWork.EntityRepository<Company>().Delete(item);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }

}
