// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.IFeedbackService
// Assembly: Yetkilim.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 14D1D813-227D-40DF-9953-925BA95C932F
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Business.dll

using System.Collections.Generic;
using System.Threading.Tasks;
using Yetkilim.Domain.DTO;
using Yetkilim.Global.Model;

namespace Yetkilim.Business.Services
{
  public interface IFeedbackService
  {
    Task<Result<FeedbackDTO>> AddFeedbackAsync(FeedbackDTO feedback);

    Task<Result<List<FeedbackDTO>>> GetAllFeedbackAsync(
      FeedbackSearchModel searchModel);

    Task<Result<int>> GetFeedbackCountByUserIdAsync(int userId);
  }
}
