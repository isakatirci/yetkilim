//using Microsoft.AspNetCore.Mvc;
//using Yetkilim.Domain.DTO;

//namespace Yetkilim.Web.Models
//{
//    public class PlaceSearchRequestModel : QuickSearchModel
//    {
//        [FromQuery(Name = "lat")]
//        public double? Latitude { get; set; }
//        [FromQuery(Name = "lon")]
//        public double? Longitude { get; set; }


//        internal PlaceSearchModel ToPlaceSearchModel()
//        {
//            return new PlaceSearchModel
//            {
//                Page = Page ?? 1,
//                PageSize = PageSize ?? 20,
//                SearchText = SearchText,
//                Latitude = Latitude,
//                Longitude = Longitude
//            };
//        }
//    }
//}