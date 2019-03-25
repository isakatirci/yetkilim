using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Models
{
	public class RegisterViewModel
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

		public string FullName
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

		public string Phone
		{
			get;
			set;
		}

		[Required]
		public string RePassword
		{
			get;
			set;
		}

		public RegisterViewModel()
		{
		}
	}
}