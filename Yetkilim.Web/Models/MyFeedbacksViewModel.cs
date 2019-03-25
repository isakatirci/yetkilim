using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.DTO;

namespace Yetkilim.Web.Models
{
	public class MyFeedbacksViewModel
	{
		public List<FeedbackDetailDTO> Feedbacks
		{
			get;
			set;
		}

		public string FormMessage
		{
			get;
			set;
		}

		public bool HasError
		{
			get;
			set;
		}

		public MyFeedbacksViewModel()
		{
		}
	}
}