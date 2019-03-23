// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.UserService
// Assembly: Yetkilim.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 14D1D813-227D-40DF-9953-925BA95C932F
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Business.dll

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
    public class UserService : ServiceBase, IUserService
    {
        private readonly IEmailSender _emailSender;

        private readonly IYetkilimUnitOfWork _unitOfWork;

        public UserService(IEmailSender emailSender, IYetkilimUnitOfWork unitOfWork, IGlobalContext globalContext)
            : base(globalContext)
        {
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserDTO>> GetUserAsync(string email, string password)
        {
            Result<UserDTO> res = new Result<UserDTO>();
            string hashedPassword = PasswordHelper.MD5Hash(password);
            User user = await _unitOfWork.EntityRepository<User>().GetFirstAsync((User w) => w.Email == email && w.Password == hashedPassword && !w.IsDeleted, null);
            if (user == null)
            {
                return res.Fail("E-posta / Şifre bilgisini kontrol ediniz.");
            }
            UserDTO mapItem = Mapper.Map<User, UserDTO>(user);
            return res.Success(mapItem);
        }

        public async Task<Result<UserDTO>> GetUserByIdAsync(int id)
        {
            Result<UserDTO> res = new Result<UserDTO>();
            User user = await _unitOfWork.EntityRepository<User>().GetFirstAsync((User w) => w.Id == id, null);
            if (user == null)
            {
                return res.Fail("User yok!");
            }
            UserDTO mapItem = Mapper.Map<User, UserDTO>(user);
            return res.Success(mapItem);
        }

        public async Task<Result<UserDTO>> GetExternalUserAsync(string provider, string nameIdendifier)
        {
            Result<UserDTO> res = new Result<UserDTO>();
            ExternalUser externalUser = await _unitOfWork.EntityRepository<ExternalUser>().GetFirstAsync((ExternalUser w) => w.Provider == provider && w.NameIdentifier == nameIdendifier, null);
            ExternalUser extarnalUser = externalUser;
            if (extarnalUser == null)
            {
                return res.Fail("ExternalUser yok!", null, "NOT_REGISTERED");
            }
            User user = await _unitOfWork.EntityRepository<User>().GetFirstAsync((User w) => (object)w.ExternalUserId == (object)(int?)extarnalUser.Id && !w.IsDeleted, null);
            if (user == null)
            {
                return res.Fail("User yok!", null, "NOT_REGISTERED");
            }
            UserDTO mapItem = Mapper.Map<User, UserDTO>(user);
            return res.Success(mapItem);
        }

        public async Task<Result<UserDTO>> AddUserAsync(UserDTO model)
        {
            Result<UserDTO> res = new Result<UserDTO>();
            EFRepository<User> repo = _unitOfWork.EntityRepository<User>();
            bool hasEmail = !string.IsNullOrWhiteSpace(model.Email);
            if (hasEmail && await repo.GetExistsAsync((User w) => w.Email == model.Email))
            {
                return res.Fail("Bu mail (" + model.Email + ") ile kullanıcı tanımlanmış!");
            }
            if (!string.IsNullOrWhiteSpace(model.Phone) && await repo.GetExistsAsync((User w) => w.Phone == model.Phone))
            {
                return res.Fail("Bu telefon (" + model.Phone + ") ile kullanıcı tanımlanmış!");
            }
            User item = Mapper.Map<UserDTO, User>(model);
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                item.Password = PasswordHelper.MD5Hash(model.Password);
            }
            User created = await repo.CreateAsync(item);
            await _unitOfWork.SaveChangesAsync();
            if (hasEmail)
            {
                await _emailSender.Send(new string[1]
                {
                model.Email
                }, "Üyeliğiniz oluşturuldu!", "Yetkilim Üyeliğiniz oluşturuldu.");
            }
            UserDTO mapItem = Mapper.Map<User, UserDTO>(created);
            return Result.Data(mapItem);
        }

        public async Task<Result<UserDTO>> AddExternalUserAsync(string provider, string nameIdendifier, UserDTO model)
        {
            Result<UserDTO> res = new Result<UserDTO>();
            ExternalUser item = new ExternalUser
            {
                NameIdentifier = nameIdendifier,
                Provider = provider
            };
            ExternalUser externalUserRes = await _unitOfWork.EntityRepository<ExternalUser>().CreateAsync(item);
            if (externalUserRes == null)
            {
                return res.Fail("Kullanıcı oluşturulamadı!");
            }
            model.IsExternal = true;
            model.ExternalUserId = externalUserRes.Id;
            return await AddUserAsync(model);
        }

        public async Task<Result> UpdateUserAsync(int id, UserDTO model)
        {
            EFRepository<User> repo = _unitOfWork.EntityRepository<User>();
            string pass = (!string.IsNullOrWhiteSpace(model.Password)) ? PasswordHelper.MD5Hash(model.Password) : string.Empty;
            User user = repo.GetFirst((User w) => w.Id == id && (w.IsExternal || w.Password == pass) && !w.IsDeleted, null);
            if (user == null)
            {
                return Result.Fail("Kullanıcı bilgileri geçersiz. Şifrenizi kontrol edin.");
            }
            if (user.Email != model.Email && await repo.GetExistsAsync((User w) => w.Email == model.Email && !w.IsDeleted))
            {
                return Result.Fail("Bu mail (" + model.Email + ") ile kullanıcı tanımlanmış!");
            }
            user.Name = model.Name;
            user.Phone = model.Phone;
            user.Email = model.Email;
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Güncellendi!");
        }

        public async Task<Result> ChangePasswordAsync(int id, string oldPassword, string newPassword)
        {
            EFRepository<User> repo = _unitOfWork.EntityRepository<User>();
            string pass = PasswordHelper.MD5Hash(oldPassword);
            User user = repo.GetFirst((User w) => w.Id == id && w.Password == pass && !w.IsDeleted, null);
            if (user == null)
            {
                return Result.Fail("Şifrenizi kontrol ediniz.");
            }
            user.Password = PasswordHelper.MD5Hash(newPassword);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Güncellendi!");
        }
    }

}
