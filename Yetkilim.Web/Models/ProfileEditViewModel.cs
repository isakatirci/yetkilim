using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Yetkilim.Web.Models
{
    public class ProfileEditViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsExternal { get; set; }

        public bool IsSuccess { get; set; }
        public string FormMessage { get; set; }
    }
}
