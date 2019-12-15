using Microsoft.Extensions.Options;
using MyBlog.Controllers;
using MyBlog.Engine.Models;
using MyBlog.Engine.Services;
using MyBlog.Engine.Tests;
using MyLib.Web.Soe;
using Xunit;

namespace MyBlog.Tests.Controllers
{
    public class SiteMapControllerTests
    {
        [Fact]
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
