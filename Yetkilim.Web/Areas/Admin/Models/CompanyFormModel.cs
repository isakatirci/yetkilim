using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Yetkilim.Web.Areas.Admin.Models
{
    public class CompanyFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ManagerName { get; set; }
        [Required]
        public string ManagerSurname { get; set; }
        [Required]
        public string ManagerEmail { get; set; }

        public string Address { get; set; }
        public string Image { get; set; }
        public int CompanyTypeId { get; set; }

        public IFormFile LogoFile { get; set; }


        // Response
        public string FormMessage { get; set; }
        public bool IsSuccess { get; set; }
    }
}