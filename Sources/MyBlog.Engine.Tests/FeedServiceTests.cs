using MyBlog.Engine.Services;
using Xunit;

namespace MyBlog.Engine.Tests
{
    public sealed class FeedServiceTests
    {
        [Fact]
        public void GetTest1()
        {
            var feed = TestsSetvices.Current.Get<FeedService>();
            var result = feed.Get();
            Assert.NotNull(result);
        }
    }
}
