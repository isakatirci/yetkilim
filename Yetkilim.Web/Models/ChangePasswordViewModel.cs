using System.ComponentModel.DataAnnotations;

namespace Yetkilim.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string NewPasswordRe { get; set; }

        public bool IsSuccess { get; set; }
        public string FormMessage { get; set; }
    }
}