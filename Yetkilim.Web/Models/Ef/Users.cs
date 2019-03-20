using System;
using System.Collections.Generic;

namespace Yetkilim.Web.Models.Ef
{
    public partial class Users
    {
        public Users()
        {
            Feedbacks = new HashSet<Feedbacks>();
            Promotions = new HashSet<Promotions>();
        }

        public int Id { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public int? ExternalUserId { get; set; }
        public bool IsExternal { get; set; }
        public string Phone { get; set; }

        public virtual ExternalUser ExternalUser { get; set; }
        public virtual ICollection<Feedbacks> Feedbacks { get; set; }
        public virtual ICollection<Promotions> Promotions { get; set; }
    }
}
