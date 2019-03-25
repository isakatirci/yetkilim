using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.DTO;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class PromotionFormModel
	{
		public DateTime DueDate
		{
			get;
			set;
		}

		[Required]
		public string DueDateStr
		{
			get;
			set;
		}

		public string FormMessage
		{
			get;
			set;
		}

		public bool IsActive
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
		public string Message
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

		[Required]
		public int UserId
		{
			get;
			set;
		}

		public List<UserDTO> Users
		{
			get;
			set;
		}

		public PromotionFormModel()
		{
		}
	}
}