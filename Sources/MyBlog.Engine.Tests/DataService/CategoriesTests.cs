using System;
using System.Linq;
using Xunit;

namespace MyBlog.Engine.Tests.DataService
{
    public class CategoriesTests
    {
        [Fact]
        public void AddAndRemoveCategoryTest1()
        {
            var db = TestsSetvices.Current.Get<Services.DataService>();
            var category = new Data.Models.Category { Name = ("Test " + Guid.NewGuid().ToString()).Substring(0,40) };
            if (db.AddCategory(category))
            {
                // Get from db
                category = db.GetCategories().FirstOrDefault(c => c.Name == category.Name);
                // Remove
                Assert.True(db.RemoveCategory(category));
            }
            else
            {
                throw new Exception("AddCategory() returned false");
            }
        }

        [Fact]
        public void GetCategoriesAndCreatIfNotExistsTest1()
        {
            var db = TestsSetvices.Current.Get<Services.DataService>();
            String category1 = "Catégorie 1";
            String category2 = "C@t€gor# 2";
            var categories = db.GetCategoriesAndCreatIfNotExists(new[] { category1, category2 });
            Assert.Equal(2, categories.Length);
            Assert.Equal(category1, categories[0].Name);
            Assert.Equal(category2, categories[1].Name);

            var result = db.GetCategories();
            Assert.NotEmpty(result);
            Assert.True(result.Length >= 2);
        }

        [Fact]
        public void GetGateoriesCountersTest1()
        {
            var db = TestsSetvices.Current.Get<Services.DataService>();
            var result = db.GetGateoriesCounters();
        }


        [Fact]
        public void GetPostsInCategory1()
        {
            var db = TestsSetvices.Current.Get<Services.DataService>();
            String category1 = "Category 1";
            var categories = db.GetCategoriesAndCreatIfNotExists(new[] { category1 });
            db.AddPost(new Data.Models.Post
            {
                Published = true,
                DateCreatedGmt = DateTime.Today.ToUniversalTime(),
                Title = "Test " + Guid.NewGuid().ToString(),
                ContentIsSplitted = true,
                BeginningOfContent = "Start " + Guid.NewGuid().ToString(),
                EndOfContent = "End " + Guid.NewGuid().ToString()
            }, categories.Select(c => c.Id).ToArray());

            var categoryId = categories[0].Id;

            var result = db.GetPostsInCategory(categoryId, 0);
            Assert.NotEmpty(result);
            Assert.True(result.Length >= 1);
        }
    }
}
