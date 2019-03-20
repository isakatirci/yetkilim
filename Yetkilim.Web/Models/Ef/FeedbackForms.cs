using System;
using System.Collections.Generic;

namespace Yetkilim.Web.Models.Ef
{
    public partial class FeedbackForms
    {
        public FeedbackForms()
        {
            Feedbacks = new HashSet<Feedbacks>();
        }

        public int Id { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int CompanyId { get; set; }
        public string FormItems { get; set; }

        public virtual Companies Company { get; set; }
        public virtual ICollection<Feedbacks> Feedbacks { get; set; }
    }
}
