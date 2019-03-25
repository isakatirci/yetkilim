using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.DTO;

namespace Yetkilim.Web.Models
{
	public class PlaceSearchResultViewModel
	{
		public string FormMessage
		{
			get;
			set;
		}

		public List<PlaceDTO> Places
		{
			get;
			set;
		}

		public string SearchText
		{
			get;
			set;
		}

		public PlaceSearchResultViewModel()
		{
		}
	}
}