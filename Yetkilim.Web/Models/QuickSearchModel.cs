using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.CompilerServices;
using Yetkilim.Global.Model;

namespace Yetkilim.Web.Models
{
	public class QuickSearchModel
	{
		[FromQuery(Name="p")]
		public int? Page
		{
			get;
			set;
		}

		[FromQuery(Name="ps")]
		public int? PageSize
		{
			get;
			set;
		}

		[FromQuery(Name="q")]
		public string SearchText
		{
			get;
			set;
		}

		public QuickSearchModel()
		{
		}

		internal SearchModel ToSearchModel()
		{
			SearchModel searchModel = new SearchModel();
			int? page = this.Page;
			searchModel.Page = (page.HasValue ? page.GetValueOrDefault() : 1);
			page = this.PageSize;
			searchModel.PageSize = (page.HasValue ? page.GetValueOrDefault() : 20);
			searchModel.SearchText = this.SearchText;
			return searchModel;
		}
	}
}