using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Models
{
	public class ProfileEditViewModel
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

		[Required]
		public string FullName
		{
			get;
			set;
		}

		public bool IsExternal
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

		public ProfileEditViewModel()
		{
		}
	}
}