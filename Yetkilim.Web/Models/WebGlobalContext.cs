using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Yetkilim.Global.Context;

namespace Yetkilim.Web.Models
{
	public class WebGlobalContext : IGlobalContext
	{
		private readonly IHttpContextAccessor _accessor;

		public string Language
		{
			get
			{
				return this._accessor.HttpContext.Items["Lang"] as string;
			}
		}

		public WebGlobalContext(IHttpContextAccessor accessor)
		{
			this._accessor = accessor;
		}
	}
}