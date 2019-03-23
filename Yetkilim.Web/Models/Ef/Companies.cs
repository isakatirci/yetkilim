using System;
using System.Collections.Generic;

namespace Yetkilim.Web.Models.Ef
{
    public partial class Companies
    {
        public Companies()
        {
            FeedbackForms = new HashSet<FeedbackForms>();
            PanelUser = new HashSet<PanelUser>();
            Places = new HashSet<Places>();
        }

        public int Id { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string Demo { get; set; }
        public int CompanyTypeId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual CompanyTypes CompanyType { get; set; }
        public virtual ICollection<FeedbackForms> FeedbackForms { get; set; }
        public virtual ICollection<PanelUser> PanelUser { get; set; }
        public virtual ICollection<Places> Places { get; set; }
    }
}
