using System;
using System.Collections.Generic;

namespace Yetkilim.Web.Models.Ef
{
    public partial class Promotions
    {
        public int Id { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string UsageCode { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public DateTime DueDate { get; set; }
        public int PlaceId { get; set; }

        public virtual Places Place { get; set; }
        public virtual Users User { get; set; }
    }
}
