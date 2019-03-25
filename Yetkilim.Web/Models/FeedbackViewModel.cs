using System;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Models
{
	public class FeedbackViewModel
	{
		public string FormData
		{
			get;
			set;
		}

		public int FormId
		{
			get;
			set;
		}

		public string FormMessage
		{
			get;
			set;
		}

		public bool IsLogged
		{
			get;
			set;
		}

		public FeedbackViewModel()
		{
		}
	}
}