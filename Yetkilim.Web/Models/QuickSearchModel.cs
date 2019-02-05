using Microsoft.AspNetCore.Mvc;
using Yetkilim.Global.Model;

namespace Yetkilim.Web.Models
{
    public class QuickSearchModel
    {
        [FromQuery(Name = "q")]
        public string SearchText { get; set; }

        [FromQuery(Name = "ps")]
        public int? PageSize { get; set; }

        [FromQuery(Name = "p")]
        public int? Page { get; set; }

        internal SearchModel ToSearchModel()
        {
            return new SearchModel
            {
                Page = Page ?? 1,
                PageSize = PageSize ?? 20,
                SearchText = SearchText
            };
        }
    }
}