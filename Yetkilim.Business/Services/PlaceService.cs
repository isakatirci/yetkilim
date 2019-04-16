// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.PlaceService
// Assembly: Yetkilim.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 14D1D813-227D-40DF-9953-925BA95C932F
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Business.dll

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
            Place place2 = Mapper.Map<PlaceDTO, Place>(place);
            if (place.Latitude.HasValue && place.Longitude.HasValue)
            {
                place2.Location = new XYPoint(place.Latitude.Value, place.Longitude.Value);
            }
            Place res = await _unitOfWork.EntityRepository<Place>().CreateAsync(place2);
            await _unitOfWork.SaveChangesAsync();
            return Result.Data(res);
        }

        public async Task<Result<List<PlaceDTO>>> GetAllPlaceAsync(PlaceSearchModel searchModel)
        {
            searchModel.FixPageDefinations();
            ////int? companyId = searchModel.CompanyId;
            ////&& ((object)companyId == null || (object)(int?)w.CompanyId == (object)companyId)
            //IQueryable<Place> source = _unitOfWork.EntityRepository<Place>().GetQueryable((Place w) => w.IsDeleted == false, null);


            int? companyId = searchModel.CompanyId;
            IQueryable<Place> source = _unitOfWork.EntityRepository<Place>().GetQueryable((Place w) => w.IsDeleted == false && ((object)companyId == null || (object)(int?)w.CompanyId == (object)companyId), null);

            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                string[] searchTexts = searchModel.SearchText.ToLower().Split(' ');
                if (searchTexts.Length == 1)
                {
                    source = from w in source
                             where w.Name.ToLower().Contains(searchTexts[0].Trim())
                             select w;
                }
                if (searchTexts.Length > 1)
                {
                    source = from w in source
                             where searchTexts.Any((string x) => w.Name.ToLower().Trim().Contains(x))
                             select w;
                }
            }
            if (searchModel.Longitude.HasValue && searchModel.Latitude.HasValue)
            {
                XYPoint sourcePoint = new XYPoint(searchModel.Latitude.Value, searchModel.Longitude.Value);
                source = from w in source
                         where w.Location != null
                         select w into o
                         orderby o.Location.Distance(sourcePoint)
                         select o;
            }
            else
            {
                source = from o in source
                         orderby o.Name
                         select o;
            }

            var sorgu = source.ToString();

            var temp  = Result.Data(await EntityFrameworkQueryableExtensions.ToListAsync<PlaceDTO>(from s in source.Skip(searchModel.PageSize * searchModel.PageIndex).Take(searchModel.PageSize)
                                                                                              select new PlaceDTO
                                                                                              {
                                                                                                  Id = s.Id,
                                                                                                  Name = s.Name,
                                                                                                  Address = s.Address,
                                                                                                  Latitude = s.Latitude,
                                                                                                  Guest = s.Guest,
                                                                                                  Longitude = s.Longitude,
                                                                                                  CompanyId = s.CompanyId,
                                                                                                  Company = new CompanyInfoDTO
                                                                                                  {
                                                                                                      Id = s.Company.Id,
                                                                                                      Name = s.Company.Name,
                                                                                                      Image = s.Company.Image,
                                                                                                      Demo = s.Company.Demo
                                                                                                  }
                                                                                              }, default(CancellationToken)));

            return temp;
        }

        public async Task<Result<PlaceDTO>> GetPlaceAsync(int id)
        {
            Result<PlaceDTO> res = new Result<PlaceDTO>();
            Place place = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Place>(_unitOfWork.EntityRepository<Place>().GetQueryable((Place w) => w.IsDeleted == false && w.Id == id, null), default(CancellationToken));
            if (place == null)
            {
                return res.Fail("Place yok!");
            }
            PlaceDTO dataVal = Mapper.Map<Place, PlaceDTO>(place);
            return res.Success(dataVal);
        }

        public async Task<Result<FeedbackFormDTO>> GetCompanyFormAsync(int placeId)
        {
            Result<FeedbackFormDTO> res = new Result<FeedbackFormDTO>();
            Place place = await _unitOfWork.EntityRepository<Place>().GetFirstAsync((Place w) => w.IsDeleted == false && w.Id == placeId, null);
            if (place == null)
            {
                return res.Fail("Place yok!");
            }
            FeedbackForm feedbackForm = await _unitOfWork.EntityRepository<FeedbackForm>().GetFirstAsync((FeedbackForm w) => w.CompanyId == place.CompanyId, null);
            if (feedbackForm == null)
            {
                return res.Fail("Form yok!");
            }
            FeedbackFormDTO dataVal = Mapper.Map<FeedbackForm, FeedbackFormDTO>(feedbackForm);
            return res.Success(dataVal);
        }

        public IQueryable<Place> GetPlaceQueryable()
        {
            return from s in _unitOfWork.EntityRepository<Place>().GetQueryable((Place w) => w.IsDeleted == false, null)
                   select new Place
                   {
                       Id = s.Id,
                       Name = s.Name,
                       Address = s.Address,
                       Guest=s.Guest,
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
            Place place2 = await _unitOfWork.EntityRepository<Place>().GetFirstAsync((Place w) => w.IsDeleted == false && w.Id == id, null);
            if (place2 == null)
            {
                return Result.Fail("Mekan bulunamadı!");
            }
            if (place.CompanyId != 0 && place2.CompanyId != place.CompanyId)
            {
                return Result.Fail("Mekan bulunamadı!");
            }

            var demoMessage = "";
            if (!string.IsNullOrWhiteSpace(place.Guest))
            {
                if ((!string.Equals("Hayir", place2.Guest, StringComparison.InvariantCultureIgnoreCase))
                    && string.Equals("Hayir", place.Guest, StringComparison.InvariantCultureIgnoreCase))
                {
                    demoMessage = "Demo";
                }
            }


            place2.Name = place.Name;
            place2.Guest = place.Guest;
            place2.Address = place.Address;
            place2.Latitude = place.Latitude;
            place2.Longitude = place.Longitude;
            if (place.Latitude.HasValue && place.Longitude.HasValue)
            {
                place2.Location = new XYPoint(place.Latitude.Value, place.Longitude.Value);
            }
            await _unitOfWork.SaveChangesAsync();
            return Result.Success(message: demoMessage);
        }

        public async Task<Result> DeletePlaceAsync(int? companyId, int id)
        {
            Place item = await _unitOfWork.EntityRepository<Place>().GetFirstAsync((Place w) => ((object)companyId == null || (object)(int?)w.CompanyId == (object)companyId) && w.Id == id, null);
            if (item != null)
            {
                List<PanelUser> list = _unitOfWork.EntityRepository<PanelUser>().GetQueryable((PanelUser w) => w.IsDeleted == false && (object)w.PlaceId == (object)(int?)id, null).ToList();
                List<Feedback> feedbacks = _unitOfWork.EntityRepository<Feedback>().GetQueryable((Feedback w) => w.IsDeleted == false && w.PlaceId == id, null).ToList();
                try
                {
                    _unitOfWork.BeginTransaction();
                    foreach (PanelUser item2 in list)
                    {
                        item2.IsDeleted = true;
                    }
                    await _unitOfWork.SaveChangesAsync();
                    foreach (Feedback item3 in feedbacks)
                    {
                        item3.IsDeleted = true;
                    }
                    await _unitOfWork.SaveChangesAsync();
                    item.IsDeleted = true;
                    await _unitOfWork.SaveChangesAsync();
                    _unitOfWork.Commit();
                    return Result.Success();
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();                  
                    return Result.Fail("İşlem sırasında transaction sorunu oluştu!");
                }
            }
            return Result.Fail("Mekan bulunamadı!");
        }

        public async Task<Result<int>> GetPlaceCountByCompanyIdAsync(int? companyId)
        {
            return Result.Data(await _unitOfWork.EntityRepository<Place>().GetCountAsync((Place w) => w.IsDeleted == false && (!companyId.HasValue || (companyId.HasValue && (object)(int?)w.CompanyId == (object)companyId))));
        }
    }


}
