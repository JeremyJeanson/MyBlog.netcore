using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Engine.Tests
{
    [TestClass]
    public sealed class SettingsTests
    {
        [TestMethod]
        public void MicrosoftAccoutnTest()
        {
            var settings = TestsSetvices.Current.Get<IOptions<Settings>>();
            Assert.IsNotNull(settings.Value.MicrosoftAccountAuthentication);
            Assert.IsTrue(settings.Value.MicrosoftAccountAuthentication.Active);
            Assert.AreEqual("a",settings.Value.MicrosoftAccountAuthentication.ClientId);
            Assert.AreEqual("b", settings.Value.MicrosoftAccountAuthentication.ClientSecret);
        }

        [TestMethod]
        public void GoogletAccoutnTest()
        {
            var settings = TestsSetvices.Current.Get<IOptions<Settings>>();
            Assert.IsNotNull(settings.Value.GoogleAuthentication);
            Assert.IsFalse(settings.Value.GoogleAuthentication.Active);
            Assert.AreEqual("yz", settings.Value.GoogleAuthentication.ClientId);
            Assert.AreEqual("zz", settings.Value.GoogleAuthentication.ClientSecret);
        }
    }
}
