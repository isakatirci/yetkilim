using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.DTO;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class Report1Model
	{
		public List<LineReportModel> FiyatModel
		{
			get;
			set;
		}

		public List<LineReportModel> HijyenModel
		{
			get;
			set;
		}

		public List<LineReportModel> IlgiModel
		{
			get;
			set;
		}

		public List<LineReportModel> LezzetModel
		{
			get;
			set;
		}

		public List<LineReportModel> Model1
		{
			get;
			set;
		}

		public List<LineReportModel> Model2
		{
			get;
			set;
		}

		public int PlaceId
		{
			get;
			set;
		}

		public List<PlaceDTO> Places
		{
			get;
			set;
		}

		public string ReportName
		{
			get;
			set;
		}

		public Report1Model()
		{
		}
	}
}