// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.IPanelUserService
// Assembly: Yetkilim.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 14D1D813-227D-40DF-9953-925BA95C932F
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Business.dll

using System.Collections.Generic;
using System.Threading.Tasks;
using Yetkilim.Domain.DTO;
using Yetkilim.Global.Model;

namespace Yetkilim.Business.Services
{
    public interface IPanelUserService
    {
        Task<Result<PanelUserDTO>> GetUserAsync(int id);

        Task<Result<PanelUserDTO>> GetUserAsync(string email, string password);

        Task<Result<PanelUserDTO>> AddUserAsync(PanelUserDTO model);

        Task<Result<List<PanelUserDTO>>> GetAllUserAsync(PanelUserSearchModel searchModel);

        Task<Result> UpdateUserAsync(int id, PanelUserDTO user);

        Task<Result> DeleteUserAsync(int id);

        Task<Result> ForgotPasswordAsync(string email);

        Task<Result> ResetPasswordAsync(string code, string email, string password);
    }
}
