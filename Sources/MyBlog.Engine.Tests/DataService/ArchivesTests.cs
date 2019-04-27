using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Engine.Tests.DataService
{
    [TestClass]
    public sealed class ArchivesTests
    {
        [TestMethod]
        public void GetArchives01()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();
            var result = db.GetArchives();
            Assert.IsNotNull(result);
        }
    }
}
