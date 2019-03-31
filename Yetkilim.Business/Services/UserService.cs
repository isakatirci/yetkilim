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
            UserDTO dataVal = Mapper.Map<User, UserDTO>(user);
            return res.Success(dataVal);
        }

        public async Task<Result<UserDTO>> GetUserByIdAsync(int id)
        {
            Result<UserDTO> res = new Result<UserDTO>();
            User user = await _unitOfWork.EntityRepository<User>().GetFirstAsync((User w) => w.Id == id, null);
            if (user == null)
            {
                return res.Fail("User yok!");
            }
            UserDTO dataVal = Mapper.Map<User, UserDTO>(user);
            return res.Success(dataVal);
        }

        public async Task<Result<UserDTO>> GetExternalUserAsync(string provider, string nameIdendifier)
        {
            Result<UserDTO> res = new Result<UserDTO>();
            ExternalUser extarnalUser = await _unitOfWork.EntityRepository<ExternalUser>().GetFirstAsync((ExternalUser w) => w.Provider == provider && w.NameIdentifier == nameIdendifier, null);
            if (extarnalUser == null)
            {
                return res.Fail("ExternalUser yok!", null, "NOT_REGISTERED");
            }
            User user = await _unitOfWork.EntityRepository<User>().GetFirstAsync((User w) => (object)w.ExternalUserId == (object)(int?)extarnalUser.Id && !w.IsDeleted, null);
            if (user == null)
            {
                return res.Fail("User yok!", null, "NOT_REGISTERED");
            }
            UserDTO dataVal = Mapper.Map<User, UserDTO>(user);
            return res.Success(dataVal);
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
            User user = Mapper.Map<UserDTO, User>(model);
            user.CreatedDate = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.Password = PasswordHelper.MD5Hash(model.Password);
            }
            User created = await repo.CreateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            if (hasEmail)
            {
                await _emailSender.Send(new string[1]
                {
                model.Email
                }, "Üyeliğiniz oluşturuldu!", "Yetkilim Üyeliğiniz oluşturuldu.");
            }
            return Result.Data(Mapper.Map<User, UserDTO>(created));
        }

        public async Task<Result<UserDTO>> AddExternalUserAsync(string provider, string nameIdendifier, UserDTO model)
        {
            Result<UserDTO> res = new Result<UserDTO>();
            ExternalUser entity = new ExternalUser
            {
                NameIdentifier = nameIdendifier,
                Provider = provider
            };
            ExternalUser externalUser = await _unitOfWork.EntityRepository<ExternalUser>().CreateAsync(entity);
            if (externalUser == null)
            {
                return res.Fail("Kullanıcı oluşturulamadı!");
            }
            model.IsExternal = true;
            model.ExternalUserId = externalUser.Id;
            return await AddUserAsync(model);
        }

        public async Task<Result> UpdateUserAsync(int id, UserDTO model)
        {
            EFRepository<User> eFRepository = _unitOfWork.EntityRepository<User>();
            string pass = (!string.IsNullOrWhiteSpace(model.Password)) ? PasswordHelper.MD5Hash(model.Password) : string.Empty;
            User user = eFRepository.GetFirst((User w) => w.Id == id && (w.IsExternal || w.Password == pass) && !w.IsDeleted, null);
            if (user == null)
            {
                return Result.Fail("Kullanıcı bilgileri geçersiz. Şifrenizi kontrol edin.");
            }
            if (user.Email != model.Email && await eFRepository.GetExistsAsync((User w) => w.Email == model.Email && !w.IsDeleted))
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
            EFRepository<User> eFRepository = _unitOfWork.EntityRepository<User>();
            string pass = PasswordHelper.MD5Hash(oldPassword);
            User first = eFRepository.GetFirst((User w) => w.Id == id && w.Password == pass && !w.IsDeleted, null);
            if (first == null)
            {
                return Result.Fail("Şifrenizi kontrol ediniz.");
            }
            first.Password = PasswordHelper.MD5Hash(newPassword);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Güncellendi!");
        }
    }

}
