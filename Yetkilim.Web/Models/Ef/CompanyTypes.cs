using System;
using System.Collections.Generic;

namespace Yetkilim.Web.Models.Ef
{
    public partial class CompanyTypes
    {
        public CompanyTypes()
        {
            Companies = new HashSet<Companies>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Companies> Companies { get; set; }
    }
}
