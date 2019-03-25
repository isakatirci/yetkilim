using System.Collections.Generic;
using System.Threading.Tasks;
using Yetkilim.Domain.DTO;
using Yetkilim.Global.Model;

namespace Yetkilim.Business.Services
{
    public interface IPromotionService
    {
        Task<Result<PromotionDTO>> AddPromotionAsync(PromotionDTO feedback);

        Task<Result<List<PromotionDTO>>> GetAllPromotionAsync(CompanyUserSearchModel searchModel);

        Task<Result<int>> GetUserActivePromotionCount(int userId);

        Task<Result> UsedPromotionAsync(int id);
    }
}
