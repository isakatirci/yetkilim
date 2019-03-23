using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public string CompanyTypeName { get; set; }

        public IFormFile LogoFile { get; set; }

        public string Demo { get; set; }
        // Response
        public string FormMessage { get; set; }
        public bool IsSuccess { get; set; }

        public List<SelectListItem> YesOrNo { get; set; }
        public List<SelectListItem> CompanyTypes { get; set; }
        public CompanyFormModel()
        {
            YesOrNo = new List<SelectListItem>();
            YesOrNo.Add(new SelectListItem(text: "Evet", value: "Evet", selected: string.Equals(this.Demo, "Evet", StringComparison.InvariantCultureIgnoreCase)));
            YesOrNo.Add(new SelectListItem(text: "Hayır", value: "Hayir", selected: string.Equals(this.Demo, "Hayir", StringComparison.InvariantCultureIgnoreCase)));

            var list = new List<Tuple<string, string>> {
                Tuple.Create("1","Cafe"),
                Tuple.Create("2", "Restoran"),
                Tuple.Create("3", "Otel"),
                Tuple.Create("4", "Güzellik Salonu"),
                Tuple.Create("5", "Benzin Istasyonu"),
                Tuple.Create("6", "Kuaför"),
                Tuple.Create("7", "Seyahat Sirketi"),
                Tuple.Create("8", "Hastane"),
                Tuple.Create("9", "Magaza"),
                Tuple.Create("10", "Market"),
                Tuple.Create("11", "Event")
            };

            CompanyTypes = new List<SelectListItem>();
            CompanyTypes.AddRange(list.Select(x => new SelectListItem { Text = x.Item2, Value = x.Item1, Selected = string.Equals(x.Item1, CompanyTypeId.ToString(), StringComparison.OrdinalIgnoreCase) }));
        }

        //Id	Name	Code



    }


}