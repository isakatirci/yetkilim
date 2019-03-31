// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.PanelUserService
// Assembly: Yetkilim.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 14D1D813-227D-40DF-9953-925BA95C932F
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Business.dll

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Yetkilim.Business;
using Yetkilim.Business.Services;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Entity;
using Yetkilim.Global.Context;
using Yetkilim.Global.Helpers;
using Yetkilim.Global.Model;
using Yetkilim.Infrastructure.Data.Repository;
using Yetkilim.Infrastructure.Data.UnitOfWork;
using Yetkilim.Infrastructure.Email;

namespace Yetkilim.Business.Services
{
    public class PanelUserService : ServiceBase, IPanelUserService
    {
        private readonly IEmailSender _emailSender;

        private readonly IYetkilimUnitOfWork _unitOfWork;

        public PanelUserService(IEmailSender emailSender, IYetkilimUnitOfWork unitOfWork, IGlobalContext globalContext)
            : base(globalContext)
        {
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PanelUserDTO>> GetUserAsync(int id)
        {
            return Result.Data(Mapper.Map<PanelUser, PanelUserDTO>(await _unitOfWork.EntityRepository<PanelUser>().GetFirstAsync((PanelUser w) => w.IsDeleted == false && w.Id == id, null)));
        }

        public async Task<Result<PanelUserDTO>> GetUserAsync(string email, string password)
        {
            Result<PanelUserDTO> res = new Result<PanelUserDTO>();
            string hashedPassword = PasswordHelper.MD5Hash(password);
            PanelUser panelUser = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<PanelUser>((IQueryable<PanelUser>)EntityFrameworkQueryableExtensions.Include<PanelUser, Company>(_unitOfWork.EntityRepository<PanelUser>().GetQueryable((PanelUser w) => w.IsDeleted == false && w.Email == email && w.Password == hashedPassword && !w.IsDeleted, null), (Expression<Func<PanelUser, Company>>)((PanelUser w) => w.Company)), default(CancellationToken));
            if (panelUser == null)
            {
                return res.Fail("User yok!");
            }
            PanelUserDTO dataVal = Mapper.Map<PanelUser, PanelUserDTO>(panelUser);
            return res.Success(dataVal);
        }

        public async Task<Result<PanelUserDTO>> AddUserAsync(PanelUserDTO model)
        {
            try
            {
                Result<PanelUserDTO> res = new Result<PanelUserDTO>();
                EFRepository<PanelUser> repo = _unitOfWork.EntityRepository<PanelUser>();
                if (await repo.GetExistsAsync((PanelUser w) => w.IsDeleted == false && w.Email == model.Email))
                {
                    return res.Fail("Bu mail (" + model.Email + ") ile kullanıcı tanımlanmış!");
                }
                PanelUser panelUser = Mapper.Map<PanelUserDTO, PanelUser>(model);
                string pass = PasswordHelper.GeneratePassword(6);
                panelUser.Password = PasswordHelper.MD5Hash(pass);
                panelUser.CreatedDate = DateTime.UtcNow;
                PanelUser created = await repo.CreateAsync(panelUser);
                await _unitOfWork.SaveChangesAsync();
                await _emailSender.Send(new string[1]
                {
        model.Email
                }, "Üyeliğiniz oluşturuldu!", "Yetkilim panele giriş şifreniz: " + pass);
                return Result.Data(Mapper.Map<PanelUser, PanelUserDTO>(created));
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

        public async Task<Result<List<PanelUserDTO>>> GetAllUserAsync(PanelUserSearchModel searchModel)
        {
            searchModel.FixPageDefinations();
            int? companyId = searchModel.CompanyId;
            IQueryable<PanelUser> source = _unitOfWork.EntityRepository<PanelUser>().GetQueryable((PanelUser w) => w.IsDeleted == false && ((object)companyId == null || (object)(int?)w.CompanyId == (object)companyId), null);
            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                source = from w in source
                         where w.Name.Contains(searchModel.SearchText) || w.Email.Contains(searchModel.SearchText)
                         select w;
            }
            return Result.Data(await EntityFrameworkQueryableExtensions.ToListAsync<PanelUserDTO>(from s in (from o in source
                                                                                                             orderby o.Id
                                                                                                             select o).Skip(searchModel.PageSize * searchModel.PageIndex).Take(searchModel.PageSize)
                                                                                                  select new PanelUserDTO
                                                                                                  {
                                                                                                      Id = s.Id,
                                                                                                      CompanyId = s.CompanyId,
                                                                                                      Role = s.Role,
                                                                                                      Name = s.Name,
                                                                                                      Email = s.Email,
                                                                                                      PlaceName = (s.PlaceId.HasValue ? s.Place.Name : null),
                                                                                                      Company = new CompanyInfoDTO
                                                                                                      {
                                                                                                          Name = s.Company.Name
                                                                                                      },
                                                                                                      CreatedDate = s.CreatedDate
                                                                                                  }, default(CancellationToken)));
        }

        public async Task<Result> UpdateUserAsync(int id, PanelUserDTO user)
        {
            EFRepository<PanelUser> repo = _unitOfWork.EntityRepository<PanelUser>();
            PanelUser item = await repo.GetFirstAsync((PanelUser w) => w.IsDeleted == false && w.Id == id, null);
            if (item == null)
            {
                return Result.Fail("Kullanıcı bulunamadı!");
            }
            item.Name = user.Name;
            item.PlaceId = user.PlaceId;
            item.Role = user.Role;
            item.ModifiedDate = DateTime.UtcNow;
            if (item.Email != user.Email)
            {
                if (await repo.GetExistsAsync((PanelUser w) => w.IsDeleted == false && w.Email == user.Email))
                {
                    return Result.Fail("Bu mail (" + user.Email + ") ile kullanıcı tanımlanmış!");
                }
                item.Email = user.Email;
                string text = PasswordHelper.GeneratePassword(6);
                item.Password = PasswordHelper.MD5Hash(text);
                await _emailSender.Send(new string[1]
                {
                user.Email
                }, "Üyeliğiniz oluşturuldu!", "Yetkilim panele giriş şifreniz: " + text);
            }
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(int id)
        {
            PanelUser panelUser = await _unitOfWork.EntityRepository<PanelUser>().GetFirstAsync((PanelUser w) => w.Id == id, null);
            if (panelUser == null)
            {
                return Result.Fail("Kullanıcı bulunamadı!");
            }
            panelUser.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> ForgotPasswordAsync(string email)
        {
            PanelUser panelUser = await _unitOfWork.EntityRepository<PanelUser>().GetFirstAsync((PanelUser w) => w.IsDeleted == false && w.Email == email, null);
            if (panelUser == null)
            {
                return Result.Success("Talebiniz alındı!");
            }
            Guid code = Guid.NewGuid();
            panelUser.ResetCode = code.ToString();
            await _unitOfWork.SaveChangesAsync();
            await _emailSender.Send(new string[1]
            {
            email
            }, "Şifre sıfırlama!", $"Yetkilim panel şifre sıfırlama talebiniz alındı. <br/> Sıfırlamak için : https://yetkilim.com/Admin/Manage/ResetPassword?code={code}");
            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(string code, string email, string password)
        {
            PanelUser panelUser = await _unitOfWork.EntityRepository<PanelUser>().GetFirstAsync((PanelUser w) => w.IsDeleted == false && w.Email == email && w.ResetCode == code, null);
            if (panelUser == null)
            {
                return Result.Fail("Talep geçersiz!");
            }
            panelUser.ResetCode = null;
            panelUser.Password = PasswordHelper.MD5Hash(password);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }

}
