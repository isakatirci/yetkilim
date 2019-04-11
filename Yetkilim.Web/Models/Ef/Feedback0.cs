using System;
using System.Collections.Generic;

namespace Yetkilim.Web.Models.Ef
{
    public partial class Feedback0
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LikeRate { get; set; }
        public int? AdviseRate { get; set; }
        public int? CleaningRate { get; set; }
        public int? EmployeeRate { get; set; }
        public int? FlavorRate { get; set; }
        public int? PriceRate { get; set; }
        public string DeskCode { get; set; }
        public string IpAddress { get; set; }
        public string Description { get; set; }
        public string FormValue { get; set; }
        public bool? IsUserShare { get; set; }
        public int? DetailId { get; set; }
        public int? FormId { get; set; }
        public int? PlaceId { get; set; }
        public int? UserId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string BrowserFp { get; set; }
        public bool IsDeleted { get; set; }
        public string UserFullName { get; set; }
        public string UserPhone { get; set; }
        public string UserAddress { get; set; }
        public string UserMail { get; set; }
    }
}
