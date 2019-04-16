using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Enums;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class PanelUserFormModel
	{
		public int? CompanyId
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

		public bool IsProfile
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

		[Required]
		public string Name
		{
			get;
			set;
		}

		public string NewPassword
		{
			get;
			set;
		}

		public string OldPassword
		{
			get;
			set;
		}

		public List<CompanyInfoDTO> Companies
		{
			get;
			set;
		}


		public int? PlaceId
		{
			get;
			set;
		}

		public List<PlaceDTO> Places
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

		public PanelUserFormModel()
		{
		}
	}
}