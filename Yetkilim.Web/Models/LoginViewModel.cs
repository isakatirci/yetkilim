using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Models
{
	public class LoginViewModel
	{
		[Required]
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

		public bool IsFeedbackRedirect
		{
			get;
			set;
		}

		public bool IsRemember
		{
			get;
			set;
		}

		[Required]
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

		public LoginViewModel()
		{
		}
	}
}