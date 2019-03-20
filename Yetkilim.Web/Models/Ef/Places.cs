using System;
using System.Collections.Generic;
using GeoAPI.Geometries;

namespace Yetkilim.Web.Models.Ef
{
    public partial class Places
    {
        public Places()
        {
            Feedbacks = new HashSet<Feedbacks>();
            PanelUser = new HashSet<PanelUser>();
            Promotions = new HashSet<Promotions>();
        }

        public int Id { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int CompanyId { get; set; }
        public IGeometry Location { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Companies Company { get; set; }
        public virtual ICollection<Feedbacks> Feedbacks { get; set; }
        public virtual ICollection<PanelUser> PanelUser { get; set; }
        public virtual ICollection<Promotions> Promotions { get; set; }
    }
}
