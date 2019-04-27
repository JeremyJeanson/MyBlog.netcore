using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Engine.Tests.DataService
{
    [TestClass]
    public sealed class PostsTests
    {
        [TestMethod]
        public void CreatePostTest1()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();
            var categories = db.GetCategoriesAndCreatIfNotExists(new[] { "CreatePost Test1" });

            var result = db.AddPost(new Data.Models.Post
            {
                Title = $"Title test {DateTime.Now.ToLongDateString()}",
                DateCreatedGmt = DateTime.UtcNow.AddMinutes(-5),
                BeginningOfContent = $"<p>Begin content test {Guid.NewGuid()}</p>",
                EndOfContent = $"<p>End content test {Guid.NewGuid()}</p>",
                ContentIsSplitted = true,
                Published = true
            }, categories.Select(c => c.Id).ToArray());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetPostsTest1()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();
            
            var posts = db.GetPosts(0);
            
            Assert.IsNotNull(posts);
            Assert.IsTrue(posts.Length > 0);
        }

        [TestMethod]
        public void GetPostWithDetailsTest1()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();
            
            var posts = db.GetPosts(0);

            Assert.IsNotNull(posts);
            Assert.IsTrue(posts.Length > 0);
            var post = db.GetPostWithDetails(posts[0].Id);
            Assert.IsNotNull(post);
            Assert.AreEqual(posts[0].Id, post.Id);
        }

        [TestMethod]
        public void GetPreviousPostTest1()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();

            var posts = db.GetPosts(0);

            Assert.IsNotNull(posts);
            Assert.IsTrue(posts.Length > 0);
            var post = db.GetPreviousPost(posts[0].Id, posts[0].DateCreatedGmt);
            Assert.IsNotNull(post);
            Assert.AreEqual(posts[1].Id, post.Id);
        }

        [TestMethod]
        public void GetPreviousPostTest2()
        {
            // Create 3 posts
            CreatePostTest1();
            CreatePostTest1();
            CreatePostTest1();

            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();

            var posts = db.GetPosts(0);

            Assert.IsNotNull(posts);
            Assert.IsTrue(posts.Length > 0);
            if (posts.Length > 1)
            {
                var post = db.GetPreviousPost(posts[1].Id, posts[1].DateCreatedGmt);
                Assert.IsNotNull(post);
                if (posts.Length > 2)
                {
                    Assert.AreEqual(posts[2].Id, post.Id);
                }
            }
            else
            {
                Assert.Inconclusive("Need more posts for this test");
            }
        }

        [TestMethod]
        public void GetNextPostTest1()
        {
            // Create 2 posts
            CreatePostTest1();
            CreatePostTest1();

            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();

            var posts = db.GetPosts(0);

            Assert.IsNotNull(posts);
            Assert.IsTrue(posts.Length > 0);
            var post = db.GetNextPost(posts[0].Id, posts[0].DateCreatedGmt);
            Assert.IsNull(post);
        }

        [TestMethod]
        public void GetNextPostTest2()
        {
            CreatePostTest1();
            CreatePostTest1();

            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();

            var posts = db.GetPosts(0);

            Assert.IsNotNull(posts);
            Assert.IsTrue(posts.Length > 0);
            if (posts.Length > 1)
            {
                var post = db.GetNextPost(posts[1].Id, posts[1].DateCreatedGmt);
                Assert.IsNotNull(post);
                Assert.AreEqual(posts[0].Id, post.Id);
            }
        }

        [TestMethod]
        public void GetPostsInSearchTest1()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();

            var posts = db.GetPostsInSearch("test", 0);
            Assert.IsNotNull(posts);
            if (posts.Length == 0)
            {
                Assert.Inconclusive("Need more posts for this test");
            }
        }

        [TestMethod]
        public void GetPostsInSearchTest2()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();

            var posts = db.GetPostsInSearch("golo golo", 0);
            Assert.IsNotNull(posts);
            if (posts.Length > 0)
            {
                Assert.Inconclusive("Strange data ;)");
            }
        }
    }
}
