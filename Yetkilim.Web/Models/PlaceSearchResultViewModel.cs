using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yetkilim.Domain.DTO;

namespace Yetkilim.Web.Models
{
    public class PlaceSearchResultViewModel
    {
        public string SearchText { get; set; }
        public List<PlaceDTO> Places { get; set; }

        public string FormMessage { get; set; }
    }
}
