using System;
using System.Collections.Generic;

namespace Yetkilim.Web.Models.Ef
{
    public partial class Feedback5
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LikeRate { get; set; }
        public int? DetailId { get; set; }
        public int? AdviseRate { get; set; }
        public int? CleaningRate { get; set; }
        public int? DoktorUzmanligi { get; set; }
        public int? EmployeeRate { get; set; }
        public int? UzmanCesidi { get; set; }
        public string Description { get; set; }
        public string DeskCode { get; set; }
        public string IpAddress { get; set; }
        public bool? IsUserShare { get; set; }
        public string FormValue { get; set; }
        public int? FormId { get; set; }
        public int? UserId { get; set; }
        public int? PlaceId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string BrowserFp { get; set; }
    }
}
