using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLib.Web.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib.Web.Tests.TagHelpers
{
    [TestClass]
    public sealed class PaginationTests
    {
        private class UrlHelperMock : IUrlHelper
        {
            public ActionContext ActionContext => throw new NotImplementedException();

            public string Action(UrlActionContext actionContext)
            {
                var values = (Microsoft.AspNetCore.Routing.RouteValueDictionary)actionContext.Values;
                // throw new NotImplementedException();
                return $"/{values.Values.ElementAt(0)}/";
            }

            public string Content(string contentPath)
            {
                throw new NotImplementedException();
            }

            public bool IsLocalUrl(string url)
            {
                throw new NotImplementedException();
            }

            public string Link(string routeName, object values)
            {
                return $"/{routeName}/{values}";
            }

            public string RouteUrl(UrlRouteContext routeContext)
            {
                throw new NotImplementedException();
            }
        }

        //[TestMethod]
        //public void GetHtmlTest1()
        //{
        //    var p = new Pagination(new UrlHelperMock());
        //    var result = p.GetHtml("Index", 100, 0, 10, null);
        //    Assert.IsFalse(String.IsNullOrWhiteSpace(result));
        //}
        // TODO : create new test for tag helpers an htmlhelpers
    }
}
