using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.DTO;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class DashboardViewModel
	{
		public int FeedbackCount
		{
			get;
			set;
		}

		public List<FeedbackDTO> Feedbacks
		{
			get;
			set;
		}

		public int PlaceCount
		{
			get;
			set;
		}

		public DashboardViewModel()
		{
		}
	}
}