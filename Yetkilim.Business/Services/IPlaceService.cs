// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.IPlaceService
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
  public interface IPlaceService
  {
    Task<Result<Place>> AddPlaceAsync(PlaceDTO place);

    Task<Result<List<PlaceDTO>>> GetAllPlaceAsync(PlaceSearchModel searchModel);

    Task<Result<PlaceDTO>> GetPlaceAsync(int id);

    Task<Result<FeedbackFormDTO>> GetCompanyFormAsync(int placeId);

    IQueryable<Place> GetPlaceQueryable();

    Task<Result> UpdatePlaceAsync(int id, PlaceDTO place);

    Task<Result> DeletePlaceAsync(int id);

    Task<Result<int>> GetPlaceCountByCompanyIdAsync(int companyId);
  }
}
