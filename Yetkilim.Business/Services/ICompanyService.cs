// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.ICompanyService
// Assembly: Yetkilim.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 14D1D813-227D-40DF-9953-925BA95C932F
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Business.dll

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Entity;
using Yetkilim.Global.Model;

namespace Yetkilim.Business.Services
{
  public interface ICompanyService
  {
    Task<Result<CompanyDetailDTO>> AddCompanyAsync(
      CompanyDetailDTO company,
      PanelUserDTO manager);

    IQueryable<Company> GetCompanyQueryable();

    Task<Result<List<CompanyInfoDTO>>> GetAllCompanyAsync(
      SearchModel searchModel);

    Task<Result<CompanyDetailDTO>> GetCompanyAsync(int id);

    Task<Result> UpdateCompanyAsync(int id, CompanyDetailDTO company);

    Task<Result> DeleteCompanyAsync(int id);
  }
}
