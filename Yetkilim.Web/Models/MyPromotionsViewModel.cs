using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Models
{
	public class MyPromotionsViewModel
	{
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

		public List<MyPromotionViewItem> Promotions
		{
			get;
			set;
		}

		public MyPromotionsViewModel()
		{
		}
	}
}