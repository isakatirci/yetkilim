using Microsoft.AspNetCore.Http;
using Yetkilim.Global.Context;

namespace Yetkilim.Web.Models
{
    public class WebGlobalContext : IGlobalContext
    {
        private readonly IHttpContextAccessor _accessor;

        public WebGlobalContext(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Language => _accessor.HttpContext.Items["Lang"] as string;
    }
}