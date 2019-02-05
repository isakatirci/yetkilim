using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yetkilim.Domain.Enums;

namespace Yetkilim.Web.Models
{
    public class CookieUserModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        //public UserRole Role { get; set; }
    }
}
