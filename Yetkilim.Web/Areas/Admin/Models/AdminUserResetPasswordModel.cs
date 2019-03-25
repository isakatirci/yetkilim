using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class AdminUserResetPasswordModel
	{
		public string Code
		{
			get;
			set;
		}

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

		public bool IsSuccess
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

		[Required]
		public string PasswordRe
		{
			get;
			set;
		}

		public AdminUserResetPasswordModel()
		{
		}
	}
}