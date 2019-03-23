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

namespace Yetkilim.Business.Services
{
    public class FeedbackService : ServiceBase, IFeedbackService
    {
        private readonly IYetkilimUnitOfWork _unitOfWork;

        public FeedbackService(IYetkilimUnitOfWork unitOfWork, IGlobalContext globalContext)
            : base(globalContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<FeedbackDTO>> AddFeedbackAsync(FeedbackDTO feedback)
        {
            Feedback item = Mapper.Map<FeedbackDTO, Feedback>(feedback);
            Feedback res = await _unitOfWork.EntityRepository<Feedback>().CreateAsync(item);
            await _unitOfWork.SaveChangesAsync();
            FeedbackDTO mapItem = Mapper.Map<Feedback, FeedbackDTO>(res);
            return Result.Data(mapItem);
        }

        public async Task<Result<List<FeedbackDTO>>> GetAllFeedbackAsync(FeedbackSearchModel searchModel)
        {
            searchModel.FixPageDefinations();
            int companyId = searchModel.CompanyId;
            int? placeId = searchModel.PlaceId;
            int? userId = searchModel.UserId;
            IQueryable<Feedback> query = _unitOfWork.EntityRepository<Feedback>().GetQueryable((Feedback w) => (placeId.HasValue && (object)(int?)w.PlaceId == (object)placeId) || (userId.HasValue && (object)w.UserId == (object)userId) || w.Place.CompanyId == companyId, null);
            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                query = from w in query
                        where w.Description.Contains(searchModel.SearchText)
                        select w;
            }
            return Result.Data(await EntityFrameworkQueryableExtensions.ToListAsync<FeedbackDTO>(from s in (from o in query
                                                                                                            orderby o.Id
                                                                                                            select o).Skip(searchModel.PageSize * searchModel.PageIndex).Take(searchModel.PageSize)
                                                                                                 select new FeedbackDTO
                                                                                                 {
                                                                                                     Id = s.Id,
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

        public async Task<Result<int>> GetFeedbackCountByUserIdAsync(int userId)
        {
            return Result.Data(await _unitOfWork.EntityRepository<Feedback>().GetCountAsync((Feedback w) => (object)w.UserId == (object)(int?)userId));
        }
    }
}
