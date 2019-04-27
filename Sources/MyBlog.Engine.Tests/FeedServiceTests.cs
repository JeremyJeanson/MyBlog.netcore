using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Engine.Tests
{
    [TestClass]
    public sealed class FeedServiceTests
    {
        [TestMethod]
        public void GetTest1()
        {
            var feed = TestsSetvices.Current.Get<FeedService>();
            var result = feed.Get();
            Assert.IsNotNull(result);
        }
    }
}
