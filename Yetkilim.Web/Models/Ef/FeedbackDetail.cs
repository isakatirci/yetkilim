using System;
using System.Collections.Generic;

namespace Yetkilim.Web.Models.Ef
{
    public partial class FeedbackDetail
    {
        public FeedbackDetail()
        {
            Feedbacks = new HashSet<Feedbacks>();
        }

        public int Id { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int EmployeeRate { get; set; }
        public int FlavorRate { get; set; }
        public int PriceRate { get; set; }
        public int CleaningRate { get; set; }
        public int AdviseRate { get; set; }

        public virtual ICollection<Feedbacks> Feedbacks { get; set; }
    }
}
