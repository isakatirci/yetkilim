namespace Yetkilim.Web.Areas.Admin.Models
{
    public class AdminLoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        public string FormMessage { get; set; }
    }
}