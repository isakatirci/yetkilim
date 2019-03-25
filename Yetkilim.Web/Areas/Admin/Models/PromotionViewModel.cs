using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.DTO;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class PromotionViewModel
	{
		public List<PlaceDTO> Places
		{
			get;
			set;
		}

		public List<PromotionDTO> Promotions
		{
			get;
			set;
		}

		public PromotionViewModel()
		{
		}
	}
}