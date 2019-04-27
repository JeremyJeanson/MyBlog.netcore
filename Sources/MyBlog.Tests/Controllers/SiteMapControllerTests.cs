using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlog.Controllers;
using MyBlog.Engine;
using MyBlog.Engine.Tests;
using MyLib.Web.SoeSiteMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Tests.Controllers
{
    [TestClass]
    public class SiteMapControllerTests
    {
        [TestMethod]
        public void IndexTest()
        {
            var controller = new SiteMapController(
                TestsSetvices.Current.Get<IOptions<Settings>>(),
                TestsSetvices.Current.Get<DataService>());
            var result = controller.Index();
            var sitemap = (SoeSiteMapResult)result;
        }
    }
}
