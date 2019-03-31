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
                Company entity = Mapper.Map<CompanyDetailDTO, Company>(company);
                Company created = await _unitOfWork.EntityRepository<Company>().CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                manager.CompanyId = created.Id;
                manager.Role = UserRole.Admin;
                Result<PanelUserDTO> result = await _panelUserService.AddUserAsync(manager);
                if (!result.IsSuccess)
                {
                    _unitOfWork.Rollback();
                    return res.Fail(result.Messages);
                }
                _unitOfWork.Commit();
                return Result.Data(Mapper.Map<Company, CompanyDetailDTO>(created));
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public IQueryable<Company> GetCompanyQueryable()
        {
            return _unitOfWork.EntityRepository<Company>().GetQueryable((Company w) => w.IsDeleted == false, null);
        }

        public async Task<Result<List<CompanyInfoDTO>>> GetAllCompanyAsync(SearchModel searchModel)
        {
            searchModel.FixPageDefinations();
            IQueryable<Company> source = _unitOfWork.EntityRepository<Company>().GetQueryable((Company w) => w.IsDeleted == false, null);
            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                source = from w in source
                         where w.Name.Contains(searchModel.SearchText)
                         select w;
            }
            return Result.Data(await EntityFrameworkQueryableExtensions.ToListAsync<CompanyInfoDTO>(from s in (from o in source
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
            return Result.Data(Mapper.Map<Company, CompanyDetailDTO>(await _unitOfWork.EntityRepository<Company>().GetFirstAsync((Company w) => w.IsDeleted == false && w.Id == id, null)));
        }

        public async Task<Result> UpdateCompanyAsync(int id, CompanyDetailDTO company)
        {
            Company company2 = await _unitOfWork.EntityRepository<Company>().GetFirstAsync((Company w) => w.IsDeleted == false && w.Id == id, null);
            if (company2 == null)
            {
                return Result.Fail("Firma bulunamadı!");
            }
            company2.Name = company.Name;
            company2.Address = company.Address;
            company2.CompanyTypeId = company.CompanyTypeId;
            if (!string.IsNullOrWhiteSpace(company.Image))
            {
                company2.Image = company.Image;
            }
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteCompanyAsync(int id)
        {
            Company item = await _unitOfWork.EntityRepository<Company>().GetFirstAsync((Company w) => w.IsDeleted == false && w.Id == id, null);
            if (item != null)
            {
                List<PanelUser> list = _unitOfWork.EntityRepository<PanelUser>().GetQueryable((PanelUser w) => w.IsDeleted == false && w.CompanyId == id, null).ToList();
                List<Place> places = ((IEnumerable<Place>)EntityFrameworkQueryableExtensions.Include<Place, ICollection<Feedback>>(_unitOfWork.EntityRepository<Place>().GetQueryable((Place w) => w.CompanyId == id, null), (Expression<Func<Place, ICollection<Feedback>>>)((Place i) => i.Feedbacks))).ToList();
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    foreach (PanelUser item2 in list)
                    {
                        item2.IsDeleted = true;
                    }
                    await _unitOfWork.SaveChangesAsync();
                    foreach (Place item3 in places)
                    {
                        foreach (Feedback item4 in (from w in item3.Feedbacks
                                                    where !w.IsDeleted
                                                    select w).ToList())
                        {
                            item4.IsDeleted = true;
                        }
                        await _unitOfWork.SaveChangesAsync();
                        item3.IsDeleted = true;
                    }
                    await _unitOfWork.SaveChangesAsync();
                    item.IsDeleted = true;
                    await _unitOfWork.SaveChangesAsync();
                    _unitOfWork.Commit();
                    return Result.Success();
                }
                catch (Exception)
                {
                    _unitOfWork.Rollback();
                    return Result.Fail("İşlem sırasında transaction sorunu oluştu!");
                }
            }
            return Result.Fail("Firma bulunamadı!");
        }
    }

}
