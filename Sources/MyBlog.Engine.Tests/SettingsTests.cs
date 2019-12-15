using Microsoft.Extensions.Options;
using MyBlog.Engine.Models;
using Xunit;

namespace MyBlog.Engine.Tests
{
    public sealed class SettingsTests
    {
        [Fact]
        public void MicrosoftAccoutnTest()
        {
            var settings = TestsSetvices.Current.Get<IOptions<Settings>>();
            Assert.NotNull(settings.Value.MicrosoftAccountAuthentication);
            Assert.True(settings.Value.MicrosoftAccountAuthentication.Active);
            Assert.Equal("a",settings.Value.MicrosoftAccountAuthentication.ClientId);
            Assert.Equal("b", settings.Value.MicrosoftAccountAuthentication.ClientSecret);
        }

        [Fact]
        public void GoogletAccoutnTest()
        {
            var settings = TestsSetvices.Current.Get<IOptions<Settings>>();
            Assert.NotNull(settings.Value.GoogleAuthentication);
            Assert.False(settings.Value.GoogleAuthentication.Active);
            Assert.Equal("yz", settings.Value.GoogleAuthentication.ClientId);
            Assert.Equal("zz", settings.Value.GoogleAuthentication.ClientSecret);
        }
    }
}
