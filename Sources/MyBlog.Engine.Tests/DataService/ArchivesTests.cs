using Xunit;

namespace MyBlog.Engine.Tests.DataService
{
    public sealed class ArchivesTests
    {
        [Fact]
        public void GetArchives01()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.Services.DataService>();
            var result = db.GetArchives();
            Assert.NotNull(result);
        }
    }
}
