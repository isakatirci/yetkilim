using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yetkilim.Web.Models
{
    public class ProfileViewModel
    {
        public string FullName { get; set; }
        public bool IsExternal { get; set; }

        public string FormMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public int FeedbackCount { get; set; }
    }
}
