// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.FeedbackService
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
using Yetkilim.Infrastructure.Data.UnitOfWork;
using Yetkilim.Infrastructure.Email;

namespace Yetkilim.Business.Services
{
    public class FeedbackService : ServiceBase, IFeedbackService
    {
        private readonly IYetkilimUnitOfWork _unitOfWork;

        private readonly IEmailSender _emailSender;

        public FeedbackService(IYetkilimUnitOfWork unitOfWork, IGlobalContext globalContext, IEmailSender emailSender)
            : base(globalContext)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public async Task<Result<FeedbackDTO>> AddFeedbackAsync(FeedbackDTO feedback)
        {
            Feedback entity = Mapper.Map<FeedbackDTO, Feedback>(feedback);
            Feedback res = await _unitOfWork.EntityRepository<Feedback>().CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            Place place = await _unitOfWork.EntityRepository<Place>().GetFirstAsync((Place w) => w.Id == feedback.PlaceId, null);
            string[] toAddresses = (from s in _unitOfWork.EntityRepository<PanelUser>().GetQueryable((PanelUser w) => ((int)w.Role == 3 && (object)w.PlaceId == (object)(int?)place.Id) || ((int)w.Role == 2 && w.CompanyId == place.CompanyId), null).ToList()
                                    select s.Email).Distinct().ToArray();
            await _emailSender.Send(toAddresses, "Firmanıza geri bildirimde bulunuldu!", place.Name + " mekanınızda bir geri bildirim iletildi. <br/> Masa Kodu: " + feedback.DeskCode + " Mesaj : " + feedback.Description);
            return Result.Data(Mapper.Map<Feedback, FeedbackDTO>(res));
        }

        public async Task<Result<List<FeedbackDTO>>> GetAllFeedbackAsync(CompanyUserSearchModel searchModel)
        {
            searchModel.FixPageDefinations();
            int? companyId = searchModel.CompanyId;
            int? placeId = searchModel.PlaceId;
            int? userId = searchModel.UserId;
            IQueryable<Feedback> source = _unitOfWork.EntityRepository<Feedback>().GetQueryable((Feedback w) => w.IsDeleted == false && ((object)placeId == null || (object)(int?)w.PlaceId == (object)placeId) && ((object)userId == null || (object)w.UserId == (object)userId) && ((object)companyId == null || (object)(int?)w.Place.CompanyId == (object)companyId), null);
            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                source = from w in source
                         where w.Description.Contains(searchModel.SearchText)
                         select w;
            }
            return Result.Data(await EntityFrameworkQueryableExtensions.ToListAsync<FeedbackDTO>(from s in (from o in source
                                                                                                            orderby o.Id descending
                                                                                                            select o).Skip(searchModel.PageSize * searchModel.PageIndex).Take(searchModel.PageSize)
                                                                                                 select new FeedbackDTO
                                                                                                 {
                                                                                                     Id = s.Id,
                                                                                                     DeskCode = s.DeskCode,
                                                                                                     Description = s.Description,
                                                                                                     CreatedDate = s.CreatedDate,
                                                                                                     CreatedBy = s.CreatedBy,
                                                                                                     UserId = s.UserId,
                                                                                                     User = (s.UserId.HasValue ? new UserInfoDTO
                                                                                                     {
                                                                                                         Id = s.User.Id,
                                                                                                         Name = s.User.Name
                                                                                                     } : null),
                                                                                                     PlaceId = s.PlaceId,
                                                                                                     Place = new PlaceInfoDTO
                                                                                                     {
                                                                                                         Id = s.Place.Id,
                                                                                                         Name = s.Place.Name
                                                                                                     }
                                                                                                 }, default(CancellationToken)));
        }

        public async Task<Result<List<FeedbackDetailDTO>>> GetAllFeedbackDetailAsync(CompanyUserSearchModel searchModel)
        {
            searchModel.FixPageDefinations();
            int? companyId = searchModel.CompanyId;
            int? placeId = searchModel.PlaceId;
            int? userId = searchModel.UserId;
            IQueryable<Feedback> source = _unitOfWork.EntityRepository<Feedback>()
                .GetQueryable((Feedback w) =>
                w.IsDeleted == false 
                && ((object)placeId == null || (object)(int?)w.PlaceId == (object)placeId) 
                && ((object)userId == null || (object)w.UserId == (object)userId) 
                && ((object)companyId == null || (object)(int?)w.Place.CompanyId == (object)companyId)
                , null);
            //IQueryable<Feedback> source = _unitOfWork.EntityRepository<Feedback>().GetQueryable((Feedback w) => w.IsDeleted == false, null);
            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                source = from w in source
                         where w.Description.Contains(searchModel.SearchText)
                         select w;
            }
            return Result.Data(await EntityFrameworkQueryableExtensions.ToListAsync<FeedbackDetailDTO>(from s in (from o in source
                                                                                                                  orderby o.Id
                                                                                                                  select o).Skip(searchModel.PageSize * searchModel.PageIndex).Take(searchModel.PageSize)
                                                                                                       select new FeedbackDetailDTO
                                                                                                       {
                                                                                                           Id = s.Id,
                                                                                                           DeskCode = s.DeskCode,
                                                                                                           Description = s.Description,
                                                                                                           CreatedDate = s.CreatedDate,
                                                                                                           PlcId = s.PlaceId,
                                                                                                           Place = s.Place.Name,
                                                                                                           Info = (s.DetailId.HasValue ? new FeedbackDetailInfoDTO
                                                                                                           {
                                                                                                               EmployeeRate = s.Detail.EmployeeRate,
                                                                                                               FlavorRate = s.Detail.FlavorRate,
                                                                                                               PriceRate = s.Detail.PriceRate,
                                                                                                               CleaningRate = s.Detail.CleaningRate,
                                                                                                               AdviseRate = s.Detail.AdviseRate
                                                                                                           } : null),
                                                                                                           User = (s.UserId.HasValue ? new UserDTO
                                                                                                           {
                                                                                                               Id = s.User.Id,
                                                                                                               Name = s.User.Name,
                                                                                                               Email = s.User.Email,
                                                                                                               Phone = s.User.Phone
                                                                                                           } : null),
                                                                                                           IsUserShare = s.IsUserShare,
                                                                                                           IsAnon = (s.UserId.HasValue == false)
                                                                                                       }, default(CancellationToken)));
        }

        public async Task<Result<int>> GetFeedbackCountByUserIdAsync(int userId)
        {
            return Result.Data(await _unitOfWork.EntityRepository<Feedback>().GetCountAsync((Feedback w) => w.IsDeleted == false && (object)w.UserId == (object)(int?)userId));
        }

        public async Task<Result<int>> GetAllFeedbackCountAsync(int? companyId, int? placeId)
        {
            return Result.Data(await _unitOfWork.EntityRepository<Feedback>().GetCountAsync((Feedback w) => w.IsDeleted == false && ((object)placeId == null || (object)(int?)w.PlaceId == (object)placeId) && ((object)companyId == null || (object)(int?)w.Place.CompanyId == (object)companyId)));
        }
    }
}
