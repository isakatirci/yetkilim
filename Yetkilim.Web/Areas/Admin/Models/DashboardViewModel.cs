using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yetkilim.Domain.DTO;

namespace Yetkilim.Web.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int FeedbackCount { get; set; }
        public int PlaceCount { get; set; }

        public List<FeedbackDTO> Feedbacks { get; set; }
    }
}
