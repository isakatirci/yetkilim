using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.DTO;
using Yetkilim.Global.Model;

namespace Yetkilim.Web.Models
{
	public class PlaceSearchRequestModel : QuickSearchModel
	{
		[FromQuery(Name="lat")]
		public double? Latitude
		{
			get;
			set;
		}

		[FromQuery(Name="lon")]
		public double? Longitude
		{
			get;
			set;
		}

		public PlaceSearchRequestModel()
		{
		}

		internal PlaceSearchModel ToPlaceSearchModel()
		{
			PlaceSearchModel placeSearchModel = new PlaceSearchModel();
			int? page = base.Page;
			placeSearchModel.Page = (page.HasValue ? page.GetValueOrDefault() : 1);
			page = base.PageSize;
			placeSearchModel.PageSize = (page.HasValue ? page.GetValueOrDefault() : 20);
			placeSearchModel.SearchText = base.SearchText;
			placeSearchModel.Latitude = this.Latitude;
			placeSearchModel.Longitude = this.Longitude;
			return placeSearchModel;
		}
	}
}