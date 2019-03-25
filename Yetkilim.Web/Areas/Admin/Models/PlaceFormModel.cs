using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.DTO;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class PlaceFormModel
	{
		public string Address
		{
			get;
			set;
		}

		public List<CompanyInfoDTO> Companies
		{
			get;
			set;
		}

		[Required]
		public int CompanyId
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

		public bool IsSuperAdmin
		{
			get;
			set;
		}

		public double? Latitude
		{
			get;
			set;
		}

		public double? Longitude
		{
			get;
			set;
		}

		[Required]
		public string Name
		{
			get;
			set;
		}

		public PlaceFormModel()
		{
		}
	}
}