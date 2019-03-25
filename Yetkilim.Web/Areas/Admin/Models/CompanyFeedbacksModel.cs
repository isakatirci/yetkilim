using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.DTO;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class CompanyFeedbacksModel
	{
		public List<FeedbackDetailDTO> Feedbacks
		{
			get;
			set;
		}

		public List<PlaceDTO> Places
		{
			get;
			set;
		}

		public CompanyFeedbacksModel()
		{
		}
	}
}