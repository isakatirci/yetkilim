using Yetkilim.Global.Model;

namespace Yetkilim.Domain.DTO
{

    public class CompanyUserSearchModel : SearchModel
    {
        public int? CompanyId
        {
            get;
            set;
        }

        public int? PlaceId
        {
            get;
            set;
        }

        public int? UserId
        {
            get;
            set;
        }
    }
}
