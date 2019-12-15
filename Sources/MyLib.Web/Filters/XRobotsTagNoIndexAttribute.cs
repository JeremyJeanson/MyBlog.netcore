using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MyLib.Web.Filters
{
    /// <summary>
    /// Allow to headers "X-Robots-Tag : noindex"
    /// </summary>
    public sealed class XRobotsTagNoIndexAttribute: ActionFilterAttribute
    {
        private const String TagName = "X-Robots-Tag";
        private const String NoIndexValue = "noindex";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Headers.Add(TagName, NoIndexValue);
        }
    }
}
