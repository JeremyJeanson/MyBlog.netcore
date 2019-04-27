using Microsoft.Extensions.Options;
using MyBlog.Engine.Data.Models;
using MyBlog.Strings;
using System;
using System.Linq;
using System.ServiceModel.Syndication;

namespace MyBlog.Engine
{
    /// <summary>
    /// Blog Feed (for RSS or ATOM)
    /// </summary>
    public sealed class FeedService
    {
        private const String MoreContentButtonFormat = "<p><a href=\"{0}\">{1} {2}</a></p>";

        private readonly Settings _options;
        private DataService _dataService;

        public FeedService(IOptions<Settings> options,DataService dataService)
        {
            _options = options.Value;
            _dataService = dataService;
        }

        /// <summary>
        /// Get the blog feed
        /// </summary>
        /// <returns></returns>
        public SyndicationFeed Get()
        {
            SyndicationFeed feed = new SyndicationFeed(
                _options.Title,
                _options.SubTitle,
                new Uri(_options.Url));

            PostWithSummary[] posts = _dataService.GetPosts(0, 10);

            if (posts != null && posts.Length > 0)
            {
                // Create a new list of items
                var items = (
                    from p in posts
                        // Let uri to use 2 times in the select
                    let uri = _dataService.GetPostUrl(p)
                    select GetItem(p, uri)
                ).ToArray();
                // Add items
                feed.Items = items;
            }

            return feed;
        }

        /// <summary>
        /// Return a item
        /// </summary>
        /// <param name="post"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static SyndicationItem GetItem(PostWithSummary post, String uri)
        {
            // Get the item
            SyndicationItem item = new SyndicationItem(
                        post.Title,
                        new TextSyndicationContent(
                            post.ContentIsSplitted
                            ? post.HtmlSummary + String.Format(MoreContentButtonFormat, uri, Resources.ReadMore, Resources.ReadMoreAboutSuffix)
                            : post.HtmlSummary,
                            TextSyndicationContentKind.XHtml),
                        new Uri(uri),
                        post.Id.ToString(),
                        post.DateCreatedGmt);

            // Add categories
            foreach(var c in post.Categories)
            {
                item.Categories.Add(new SyndicationCategory(c.Name));
            }

            // Return
            return item;
        }
    }
}
