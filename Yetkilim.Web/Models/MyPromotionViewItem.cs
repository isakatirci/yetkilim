using System;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Models
{
	public class MyPromotionViewItem
	{
		public string Description
		{
			get;
			set;
		}

		public DateTime DueDate
		{
			get;
			set;
		}

		public string Place
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}

		public MyPromotionViewItem()
		{
		}
	}
}