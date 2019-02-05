using System.ComponentModel.DataAnnotations;

namespace Yetkilim.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsFeedbackRedirect { get; set; }
        public bool IsRemember{ get; set; }
        public string ReturnUrl { get; set; }
        public string FormMessage { get; set; }
    }
}
