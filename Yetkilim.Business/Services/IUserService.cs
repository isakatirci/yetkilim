// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.IUserService
// Assembly: Yetkilim.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 14D1D813-227D-40DF-9953-925BA95C932F
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Business.dll

using System.Threading.Tasks;
using Yetkilim.Domain.DTO;
using Yetkilim.Global.Model;

namespace Yetkilim.Business.Services
{
  public interface IUserService
  {
    Task<Result<UserDTO>> GetUserAsync(string email, string password);

    Task<Result<UserDTO>> GetUserByIdAsync(int id);

    Task<Result<UserDTO>> GetExternalUserAsync(
      string provider,
      string nameIdendifier);

    Task<Result<UserDTO>> AddUserAsync(UserDTO model);

    Task<Result<UserDTO>> AddExternalUserAsync(
      string provider,
      string nameIdendifier,
      UserDTO model);

    Task<Result> UpdateUserAsync(int id, UserDTO model);

    Task<Result> ChangePasswordAsync(int id, string oldPassword, string newPassword);
  }
}
