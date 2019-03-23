// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.PlaceService
// Assembly: Yetkilim.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 14D1D813-227D-40DF-9953-925BA95C932F
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Business.dll

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Entity;
using Yetkilim.Global.Context;
using Yetkilim.Global.Model;
using Yetkilim.Infrastructure.Data.Model;
using Yetkilim.Infrastructure.Data.Repository;
using Yetkilim.Infrastructure.Data.UnitOfWork;

namespace Yetkilim.Business.Services
{

    public class PlaceService : ServiceBase, IPlaceService
    {
        private readonly IYetkilimUnitOfWork _unitOfWork;

        public PlaceService(IYetkilimUnitOfWork unitOfWork, IGlobalContext globalContext)
            : base(globalContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Place>> AddPlaceAsync(PlaceDTO place)
        {
            Place item = Mapper.Map<PlaceDTO, Place>(place);
            Place res = await _unitOfWork.EntityRepository<Place>().CreateAsync(item);
            await _unitOfWork.SaveChangesAsync();
            return Result.Data(res);
        }

        public async Task<Result<List<PlaceDTO>>> GetAllPlaceAsync(PlaceSearchModel searchModel)
        {
            searchModel.FixPageDefinations();
            IQueryable<Place> query = _unitOfWork.EntityRepository<Place>().GetQueryable(null, null);
            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                query = from w in query
                        where w.Name.Contains(searchModel.SearchText)
                        select w;
            }
            if (searchModel.Longitude.HasValue && searchModel.Latitude.HasValue)
            {
                XYPoint sourcePoint = new XYPoint(searchModel.Latitude.Value, searchModel.Longitude.Value);
                query = from o in query
                        orderby o.Location.Distance(sourcePoint)
                        select o;
            }
            else
            {
                query = from o in query
                        orderby o.Name
                        select o;
            }
            return Result.Data(await EntityFrameworkQueryableExtensions.ToListAsync<PlaceDTO>(from s in query.Skip(searchModel.PageSize * searchModel.PageIndex).Take(searchModel.PageSize)
                                                                                              select new PlaceDTO
                                                                                              {
                                                                                                  Id = s.Id,
                                                                                                  Name = s.Name,
                                                                                                  Address = s.Address,
                                                                                                  Latitude = s.Latitude,
                                                                                                  Longitude = s.Longitude,
                                                                                                  CompanyId = s.CompanyId,
                                                                                                  Company = new CompanyInfoDTO
                                                                                                  {
                                                                                                      Id = s.Company.Id,
                                                                                                      Name = s.Company.Name,
                                                                                                      Image = s.Company.Image
                                                                                                  }
                                                                                              }, default(CancellationToken)));
        }

        public async Task<Result<PlaceDTO>> GetPlaceAsync(int id)
        {
            Result<PlaceDTO> res = new Result<PlaceDTO>();
            EFRepository<Place> repo = _unitOfWork.EntityRepository<Place>();
            IQueryable<Place> query = repo.GetQueryable((Place w) => w.Id == id, null);
            Place place = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Place>(query, default(CancellationToken));
            if (place == null)
            {
                return res.Fail("Place yok!");
            }
            PlaceDTO mapItem = Mapper.Map<Place, PlaceDTO>(place);
            return res.Success(mapItem);
        }

        public async Task<Result<FeedbackFormDTO>> GetCompanyFormAsync(int placeId)
        {
            Result<FeedbackFormDTO> res = new Result<FeedbackFormDTO>();
            Place place2 = await _unitOfWork.EntityRepository<Place>().GetFirstAsync((Place w) => w.Id == placeId, null);
            Place place = place2;
            if (place == null)
            {
                return res.Fail("Place yok!");
            }
            FeedbackForm feedbackForm = await _unitOfWork.EntityRepository<FeedbackForm>().GetFirstAsync((FeedbackForm w) => w.CompanyId == place.CompanyId, null);
            if (feedbackForm == null)
            {
                return res.Fail("Form yok!");
            }
            FeedbackFormDTO mapItem = Mapper.Map<FeedbackForm, FeedbackFormDTO>(feedbackForm);
            return res.Success(mapItem);
        }

        public IQueryable<Place> GetPlaceQueryable()
        {
            return from s in _unitOfWork.EntityRepository<Place>().GetQueryable(null, null)
                   select new Place
                   {
                       Id = s.Id,
                       Name = s.Name,
                       Address = s.Address,
                       Latitude = s.Latitude,
                       Longitude = s.Longitude,
                       CreatedDate = s.CreatedDate,
                       CreatedBy = s.CreatedBy,
                       ModifiedDate = s.ModifiedDate,
                       ModifiedBy = s.ModifiedBy,
                       CompanyId = s.CompanyId,
                       Company = new Company
                       {
                           Id = s.CompanyId,
                           Name = s.Company.Name
                       }
                   };
        }

        public async Task<Result> UpdatePlaceAsync(int id, PlaceDTO place)
        {
            Place item = await _unitOfWork.EntityRepository<Place>().GetFirstAsync((Place w) => w.Id == id, null);
            if (item == null)
            {
                return Result.Fail("Mekan bulunamadı!");
            }
            item.Name = place.Name;
            item.Address = place.Address;
            item.Latitude = place.Latitude;
            item.Longitude = place.Longitude;
            item.CompanyId = place.CompanyId;
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeletePlaceAsync(int id)
        {
            Place item = await _unitOfWork.EntityRepository<Place>().GetFirstAsync((Place w) => w.Id == id, null);
            if (item == null)
            {
                return Result.Fail("Mekan bulunamadı!");
            }
            _unitOfWork.EntityRepository<Place>().Delete(item);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<int>> GetPlaceCountByCompanyIdAsync(int companyId)
        {
            return Result.Data(await _unitOfWork.EntityRepository<Place>().GetCountAsync((Place w) => w.CompanyId == companyId));
        }
    }

}
