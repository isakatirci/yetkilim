// PromotionService
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Yetkilim.Business;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Entity;
using Yetkilim.Global.Context;
using Yetkilim.Global.Model;
using Yetkilim.Infrastructure.Data.UnitOfWork;
using Yetkilim.Infrastructure.Email;

public class PromotionService : ServiceBase, IPromotionService
{
    private readonly IYetkilimUnitOfWork _unitOfWork;

    private readonly IEmailSender _emailSender;

    public PromotionService(IYetkilimUnitOfWork unitOfWork, IGlobalContext globalContext, IEmailSender emailSender)
        : base(globalContext)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
    }

    public async Task<Result<PromotionDTO>> AddPromotionAsync(PromotionDTO feedback)
    {
        Promotion promotion = Mapper.Map<PromotionDTO, Promotion>(feedback);
        Promotion res = await _unitOfWork.EntityRepository<Promotion>().CreateAsync(promotion, (string)null);
        await _unitOfWork.SaveChangesAsync();
        string name = _unitOfWork.EntityRepository<Place>().GetFirst((Expression<Func<Place, bool>>)((Place w) => w.Id == feedback.PlaceId), (Func<IQueryable<Place>, IOrderedQueryable<Place>>)null, Array.Empty<Expression<Func<Place, object>>>()).Name;
        string email = _unitOfWork.EntityRepository<User>().GetFirst((Expression<Func<User, bool>>)((User w) => w.Id == feedback.UserId), (Func<IQueryable<User>, IOrderedQueryable<User>>)null, Array.Empty<Expression<Func<User, object>>>()).Email;
        await _emailSender.Send((IEnumerable<string>)new string[1]
        {
            email
        }, "Hesabınıza promosyon tanımlandı!", name + " mekanından hesabınıza promosyon tanımlandı <br/> Promosyon: " + feedback.Message);
        return Result.Data<PromotionDTO>(Mapper.Map<Promotion, PromotionDTO>(res));
    }

    public async Task<Result<List<PromotionDTO>>> GetAllPromotionAsync(CompanyUserSearchModel searchModel)
    {
        searchModel.FixPageDefinations();
        int? companyId = searchModel.CompanyId;
        int? placeId = searchModel.PlaceId;
        int? userId = searchModel.UserId;
        IQueryable<Promotion> source = _unitOfWork.EntityRepository<Promotion>().GetQueryable((Expression<Func<Promotion, bool>>)((Promotion w) => ((object)placeId == null || (object)(int?)w.PlaceId == (object)placeId) && ((object)userId == null || (object)(int?)w.UserId == (object)userId) && ((object)companyId == null || (object)(int?)w.Place.CompanyId == (object)companyId)), (Func<IQueryable<Promotion>, IOrderedQueryable<Promotion>>)null, Array.Empty<Expression<Func<Promotion, object>>>());
        if (!string.IsNullOrEmpty(searchModel.SearchText))
        {
            source = from w in source
                     where w.Message.Contains(searchModel.SearchText)
                     select w;
        }
        DateTime today = DateTime.Today;
        return Result.Data<List<PromotionDTO>>(await EntityFrameworkQueryableExtensions.ToListAsync<PromotionDTO>(from s in (from o in source
                                                                                                                             orderby o.Id
                                                                                                                             select o).Skip(searchModel.PageSize * searchModel.PageIndex).Take(searchModel.PageSize)
                                                                                                                  select new PromotionDTO
                                                                                                                  {
                                                                                                                      Id = s.Id,
                                                                                                                      Title = s.Title,
                                                                                                                      Message = s.Message,
                                                                                                                      DueDate = s.DueDate,
                                                                                                                      IsActive = s.IsActive,
                                                                                                                      CreatedDate = s.CreatedDate,
                                                                                                                      CreatedBy = s.CreatedBy,
                                                                                                                      UsageCode = s.UsageCode,
                                                                                                                      UserId = s.UserId,
                                                                                                                      Status = ((s.DueDate < today) ? "Süresi Bitti" : (s.IsActive ? "Aktif" : "Kullanıldı")),
                                                                                                                      User = new UserInfoDTO
                                                                                                                      {
                                                                                                                          Id = s.User.Id,
                                                                                                                          Name = s.User.Name,
                                                                                                                          Email = s.User.Email
                                                                                                                      },
                                                                                                                      Place = new PlaceInfoDTO
                                                                                                                      {
                                                                                                                          Id = s.Place.Id,
                                                                                                                          Name = s.Place.Name
                                                                                                                      }
                                                                                                                  }, default(CancellationToken)));
    }

    public async Task<Result<int>> GetUserActivePromotionCount(int userId)
    {
        DateTime today = DateTime.Today;
        return Result.Data<int>(await EntityFrameworkQueryableExtensions.CountAsync<Promotion>(_unitOfWork.EntityRepository<Promotion>().GetQueryable((Expression<Func<Promotion, bool>>)((Promotion w) => w.UserId == userId && w.IsActive && w.DueDate >= today), (Func<IQueryable<Promotion>, IOrderedQueryable<Promotion>>)null, Array.Empty<Expression<Func<Promotion, object>>>()), default(CancellationToken)));
    }

    public async Task<Result> UsedPromotionAsync(int id)
    {
        DateTime today = DateTime.Today;
        Promotion item = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Promotion>((IQueryable<Promotion>)EntityFrameworkQueryableExtensions.Include<Promotion, User>((IQueryable<Promotion>)EntityFrameworkQueryableExtensions.Include<Promotion, Place>(_unitOfWork.EntityRepository<Promotion>().GetQueryable((Expression<Func<Promotion, bool>>)((Promotion w) => w.Id == id && w.IsActive && w.DueDate >= today), (Func<IQueryable<Promotion>, IOrderedQueryable<Promotion>>)null, Array.Empty<Expression<Func<Promotion, object>>>()), (Expression<Func<Promotion, Place>>)((Promotion i) => i.Place)), (Expression<Func<Promotion, User>>)((Promotion i) => i.User)), default(CancellationToken));
        if (item == null)
        {
            return Result.Fail("Aktif promosyon tanımı bulunamadı!", (Exception)null, (string)null);
        }
        item.IsActive = false;
        await _unitOfWork.SaveChangesAsync();
        await _emailSender.Send((IEnumerable<string>)new string[1]
        {
            item.User.Email
        }, "Hesabınızdaki promosyon kullanıldı!", item.Place.Name + " mekanındandaki tanımlu promosyonunuzu kullandınız. <br/> Promosyon: " + item.Message);
        return Result.Success("Başarılı");
    }
}