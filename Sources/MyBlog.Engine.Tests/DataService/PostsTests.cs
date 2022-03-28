using System;
using System.Linq;
using Xunit;
using MyBlog.Engine;
using MyBlog.Engine.Tests;

namespace Services.Tests.DataService
{
    public sealed class PostsTests
    {
        [Fact]
        public void CreatePostTest1()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.Services.DataService>();
            var categories = db.GetCategoriesAndCreatIfNotExists(new[] { "CreatePost Test1" });

            var result = db.AddPost(new MyBlog.Engine.Data.Models.Post
            {
                Title = $"Title test {DateTime.Now.ToLongDateString()}",
                DateCreatedGmt = DateTime.UtcNow.AddMinutes(-5),
                BeginningOfContent = $"<p>Begin content test {Guid.NewGuid()}</p>",
                EndOfContent = $"<p>End content test {Guid.NewGuid()}</p>",
                ContentIsSplitted = true,
                Published = true
            }, categories.Select(c => c.Id).ToArray());
            Assert.True(result);
        }

        [Fact]
        public void CreatePostTest2()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.Services.DataService>();
            var categories = db.GetCategoriesAndCreatIfNotExists(new[] { "Code" });

            var result = db.AddPost(new MyBlog.Engine.Data.Models.Post
            {
                Title = $"Code {DateTime.Now.ToLongDateString()}",
                DateCreatedGmt = DateTime.UtcNow.AddMinutes(-5),
                BeginningOfContent = $"<p>Begin content test {Guid.NewGuid()}</p><pre><code class=\"language-cs\">int i = 0;\r\ni++;\r\nConsole.WrileLine(\"i =\" + i);</code></pre><br/>",
                EndOfContent = $"<p>End content test {Guid.NewGuid()}</p>",
                ContentIsSplitted = true,
                Published = true
            }, categories.Select(c => c.Id).ToArray());
            Assert.True(result);
        }

        [Fact]
        public void CreatePostWithCode()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.Services.DataService>();
            var categories = db.GetCategoriesAndCreatIfNotExists(new[] { "Code" });

            var result = db.AddPost(new MyBlog.Engine.Data.Models.Post
            {
                Title = $"Code {DateTime.Now.ToLongDateString()}",
                DateCreatedGmt = DateTime.UtcNow.AddMinutes(-5),
                BeginningOfContent = $"<p>Begin content test {Guid.NewGuid()}</p><pre><code class=\"language-cs\">int i = 0;\r\ni++;\r\nConsole.WrileLine(\"i =\" + i);</code></pre><br/>",
                EndOfContent = $"<p>End content test {Guid.NewGuid()}</p>",
                ContentIsSplitted = true,
                Published = true
            }, categories.Select(c => c.Id).ToArray());
            Assert.True(result);
        }

        [Fact]
        public void GetPostsTest1()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.Services.DataService>();
            
            var posts = db.GetPosts(0);
            
            Assert.NotNull(posts);
            Assert.True(posts.Length > 0);
        }

        [Fact]
        public void GetPostWithDetailsTest1()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.Services.DataService>();
            
            var posts = db.GetPosts(0);

            Assert.NotNull(posts);
            Assert.True(posts.Length > 0);
            var post = db.GetPostWithDetails(posts[0].Id);
            Assert.NotNull(post);
            Assert.Equal(posts[0].Id, post.Id);
        }

        //[Fact]
        //public void GetPreviousPostTest1()
        //{
        //    var db = TestsSetvices.Current.Get<MyBlog.Engine.Services.DataService>();

        //    var posts = db.GetPosts(0);

        //    Assert.NotNull(posts);
        //    Assert.True(posts.Length > 0);
        //    var post = db.GetPreviousPost(posts[0].Id, posts[0].DateCreatedGmt);
        //    Assert.NotNull(post);
        //    Assert.Equal(posts[1].Id, post.Id);
        //}
        
        //[Fact]
        //public void GetNextPostTest1()
        //{
        //    // Create 2 posts
        //    CreatePostTest1();
        //    CreatePostTest1();

        //    var db = TestsSetvices.Current.Get<MyBlog.Engine.Services.DataService>();

        //    var posts = db.GetPosts(0);

        //    Assert.NotNull(posts);
        //    Assert.True(posts.Length > 0);
        //    var post = db.GetNextPost(posts[0].Id, posts[0].DateCreatedGmt);
        //    Assert.Null(post);
        //}

        //[Fact]
        //public void GetNextPostTest2()
        //{
        //    // Create posts
        //    CreatePostTest1();
        //    CreatePostTest1();
        //    CreatePostTest1();
        //    CreatePostTest1();

        //    var db = TestsSetvices.Current.Get<MyBlog.Engine.Services.DataService>();

        //    var posts = db.GetPosts(0);

        //    Assert.NotNull(posts);
        //    Assert.True(posts.Length > 0);
        //    if (posts.Length > 1)
        //    {
        //        var post = db.GetNextPost(posts[1].Id, posts[1].DateCreatedGmt);
        //        Assert.NotNull(post);
        //        Assert.Equal(posts[0].Id, post.Id);
        //    }
        //}

        [Fact]
        public void GetPostsInSearchTest1()
        {
            // Create posts
            CreatePostTest1();
            CreatePostTest1();
            CreatePostTest1();
            CreatePostTest1();

            var db = TestsSetvices.Current.Get<MyBlog.Engine.Services.DataService>();

            var posts = db.GetPostsInSearch("test", 0);
            Assert.NotNull(posts);
            if (posts.Length == 0)
            {
                throw new Exception("Need more posts for this test");
            }
        }

        [Fact]
        public void GetPostsInSearchTest2()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.Services.DataService>();

            var posts = db.GetPostsInSearch("golo golo", 0);
            Assert.NotNull(posts);
            if (posts.Length > 0)
            {
                throw new Exception("Strange data ;)");
            }
        }
    }
}
