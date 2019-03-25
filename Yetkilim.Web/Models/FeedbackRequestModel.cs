using System;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Models
{
	public class FeedbackRequestModel
	{
		public int AdviseRate
		{
			get;
			set;
		}

		public string BrowserFp
		{
			get;
			set;
		}

		public int CleaningRate
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string DeskCode
		{
			get;
			set;
		}

		public int EmployeeRate
		{
			get;
			set;
		}

		public int FlavorRate
		{
			get;
			set;
		}

		public int FormId
		{
			get;
			set;
		}

		public string FormValue
		{
			get;
			set;
		}

		public string IpAddress
		{
			get;
			set;
		}

		public bool IsUserShare
		{
			get;
			set;
		}

		public int PriceRate
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public FeedbackRequestModel()
		{
		}
	}
}