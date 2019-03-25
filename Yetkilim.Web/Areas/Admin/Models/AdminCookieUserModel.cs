using System;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.Enums;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class AdminCookieUserModel
	{
		public int CompanyId
		{
			get;
			set;
		}

		public string CompanyName
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string FullName
		{
			get;
			set;
		}

		public int? PlaceId
		{
			get;
			set;
		}

		public UserRole Role
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public AdminCookieUserModel()
		{
		}
	}
}