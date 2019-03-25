using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Models
{
	public class ChangePasswordViewModel
	{
		public string FormMessage
		{
			get;
			set;
		}

		public bool IsSuccess
		{
			get;
			set;
		}

		[Required]
		public string NewPassword
		{
			get;
			set;
		}

		[Required]
		public string NewPasswordRe
		{
			get;
			set;
		}

		[Required]
		public string OldPassword
		{
			get;
			set;
		}

		public ChangePasswordViewModel()
		{
		}
	}
}