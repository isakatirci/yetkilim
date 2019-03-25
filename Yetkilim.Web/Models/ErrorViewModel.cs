using System;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Models
{
	public class ErrorViewModel
	{
		public string RequestId
		{
			get;
			set;
		}

		public bool ShowRequestId
		{
			get
			{
				return !string.IsNullOrEmpty(this.RequestId);
			}
		}

		public ErrorViewModel()
		{
		}
	}
}