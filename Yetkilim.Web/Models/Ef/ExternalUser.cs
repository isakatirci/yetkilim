using System;
using System.Collections.Generic;

namespace Yetkilim.Web.Models.Ef
{
    public partial class ExternalUser
    {
        public ExternalUser()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Provider { get; set; }
        public string NameIdentifier { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
