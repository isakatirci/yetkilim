// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.PanelUserService
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

        public async Task<Result<PanelUserDTO>> GetUserAsync(string email, string password)
        {
            Result<PanelUserDTO> res = new Result<PanelUserDTO>();
            string hashedPassword = PasswordHelper.MD5Hash(password);
            PanelUser user = await _unitOfWork.EntityRepository<PanelUser>().GetFirstAsync((PanelUser w) => w.Email == email && w.Password == hashedPassword && !w.IsDeleted, null);
            if (user == null)
            {
                return res.Fail("User yok!");
            }
            PanelUserDTO mapItem = Mapper.Map<PanelUser, PanelUserDTO>(user);
            return res.Success(mapItem);
        }

        public async Task<Result<PanelUserDTO>> AddUserAsync(PanelUserDTO model)
        {
            Result<PanelUserDTO> res = new Result<PanelUserDTO>();
            EFRepository<PanelUser> repo = _unitOfWork.EntityRepository<PanelUser>();
            if (await repo.GetExistsAsync((PanelUser w) => w.Email == model.Email))
            {
                return res.Fail("Bu mail (" + model.Email + ") ile kullanıcı tanımlanmış!");
            }
            PanelUser item = Mapper.Map<PanelUserDTO, PanelUser>(model);
            string pass = PasswordHelper.GeneratePassword(6);
            item.Password = PasswordHelper.MD5Hash(pass);
            PanelUser created = await repo.CreateAsync(item);
            await _unitOfWork.SaveChangesAsync();
            await _emailSender.Send(new string[1]
            {
            model.Email
            }, "Üyeliğiniz oluşturuldu!", "Yetkilim panele giriş şifreniz: " + pass);
            PanelUserDTO mapItem = Mapper.Map<PanelUser, PanelUserDTO>(created);
            return Result.Data(mapItem);
        }
    }
}
