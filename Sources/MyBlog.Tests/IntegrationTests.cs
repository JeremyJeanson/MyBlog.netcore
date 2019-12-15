using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MyBlog.Tests
{
    public class IntegrationTests: IClassFixture<WebApplicationFactory<MyBlog.Startup>>
    {
        private const string HtmlContentType = "text/html; charset=utf-8";
        private const string XmlContentType = "text/xml";
        private const string RssContentType = "application/rss+xml";
        private const string AtomContentType = "application/atom+xml";

        private readonly WebApplicationFactory<MyBlog.Startup> _factory;
        private readonly ITestOutputHelper _output;

        public IntegrationTests(WebApplicationFactory<MyBlog.Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Theory]
        [InlineData("/", HtmlContentType)]
        [InlineData("/About", HtmlContentType)]
        [InlineData("/About/PrivacyAndUsage", HtmlContentType)]
        [InlineData("/About/Cookies", HtmlContentType)]
        [InlineData("/Mail", HtmlContentType)]
        [InlineData("/Mail/Sended", HtmlContentType)]       
        [InlineData("/Post", HtmlContentType)]
        [InlineData("/Post/Category/1", HtmlContentType)]
        //[InlineData("/Post/Details/1", HtmlContentType)]
        [InlineData("/Post/Archive/1977-5", HtmlContentType)]
        [InlineData("/Post/Search/?query=foo", HtmlContentType)]
        [InlineData("/sitemap", XmlContentType)]
        [InlineData("/feed", RssContentType)]
        [InlineData("/feed/atom", AtomContentType)]
        [InlineData("/feed/rss", RssContentType)]
        [InlineData("/UserSettings/Accessibility", HtmlContentType)]
        public async Task GetEndpoints(String url, String contentType)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert

            // Test content
            String content = await response.Content.ReadAsStringAsync();
            _output.WriteLine(content);
            Assert.False(String.IsNullOrWhiteSpace(content));

            // Test ContentType
            if (String.IsNullOrWhiteSpace(contentType))
            {
                Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
            }
            else
            {
                response.EnsureSuccessStatusCode(); // Status Code 200-299
                Assert.Equal(contentType, response.Content.Headers.ContentType.ToString());
            }
            
        }
    }
}
