using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Areas.Admin.Models
{
	public class CompanyFormModel
	{
		public string Address
		{
			get;
			set;
		}

		public int CompanyTypeId
		{
			get;
			set;
		}

		public string FormMessage
		{
			get;
			set;
		}

		public string Image
		{
			get;
			set;
		}

		public bool IsSuccess
		{
			get;
			set;
		}

		public bool IsUpdate
		{
			get;
			set;
		}

		public IFormFile LogoFile
		{
			get;
			set;
		}

		public string ManagerEmail
		{
			get;
			set;
		}

		public string ManagerName
		{
			get;
			set;
		}

		public string ManagerSurname
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

		public CompanyFormModel()
		{
		}
	}
}