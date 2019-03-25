using System;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Models
{
	public class MyFeedbackViewItem
	{
		public DateTime CreatedDate
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Place
		{
			get;
			set;
		}

		public MyFeedbackViewItem()
		{
		}
	}
}