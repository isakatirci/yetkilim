using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.DTO;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class PanelUserModel
	{
		public bool IsSuperAdmin
		{
			get;
			set;
		}

		public List<PanelUserDTO> Users
		{
			get;
			set;
		}

		public PanelUserModel()
		{
		}
	}
}