using System;
using System.Collections.Generic;

namespace Yetkilim.Web.Models.Ef
{
    public partial class PanelUser
    {
        public int Id { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public int Role { get; set; }
        public int CompanyId { get; set; }
        public int? PlaceId { get; set; }
        public string ResetCode { get; set; }

        public virtual Companies Company { get; set; }
        public virtual Places Place { get; set; }
    }
}
