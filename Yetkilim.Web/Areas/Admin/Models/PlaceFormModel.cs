using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Yetkilim.Domain.DTO;

namespace Yetkilim.Web.Areas.Admin.Models
{
    public class PlaceFormModel
    {
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public List<CompanyInfoDTO> Companies { get; set; }

        // Response
        public string FormMessage { get; set; }
        public bool IsSuccess { get; set; }
    }
}
