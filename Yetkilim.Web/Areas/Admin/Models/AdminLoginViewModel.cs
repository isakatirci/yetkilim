using System;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class AdminLoginViewModel
	{
		public string Email
		{
			get;
			set;
		}

		public string FormMessage
		{
			get;
			set;
		}

		public bool IsSuccessForgot
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public string ReturnUrl
		{
			get;
			set;
		}

		public AdminLoginViewModel()
		{
		}
	}
}