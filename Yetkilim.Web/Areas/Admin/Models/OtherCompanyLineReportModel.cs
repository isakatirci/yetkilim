using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class OtherCompanyLineReportModel
	{
		public string CompanyName
		{
			get;
			set;
		}

		public List<Yetkilim.Web.Areas.Admin.Models.LineReportModel> LineReportModel
		{
			get;
			set;
		}

		public OtherCompanyLineReportModel()
		{
		}
	}
}