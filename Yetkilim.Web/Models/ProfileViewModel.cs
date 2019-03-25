using System;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Models
{
	public class ProfileViewModel
	{
		public DateTime CreatedDate
		{
			get;
			set;
		}

		public int FeedbackCount
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

		public bool IsExternal
		{
			get;
			set;
		}

		public ProfileViewModel()
		{
		}
	}
}