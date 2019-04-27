using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlog.Engine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Engine.Tests.DataService
{
    [TestClass]
    public class CategoriesTests
    {
        [TestMethod]
        public void AddAndRemoveCategoryTest1()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();
            var category = new Data.Models.Category { Name = ("Test " + Guid.NewGuid().ToString()).Substring(0,40) };
            if (db.AddCategory(category))
            {
                // Get from db
                category = db.GetCategories().FirstOrDefault(c => c.Name == category.Name);
                // Remove
                Assert.IsTrue(db.RemoveCategory(category));
            }
            else
            {
                Assert.Fail("AddCategory() returned false");
            }
        }

        [TestMethod]
        public void GetCategoriesAndCreatIfNotExistsTest1()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();
            String category1 = "Catégorie 1";
            String category2 = "C@t€gor# 2";
            var categories = db.GetCategoriesAndCreatIfNotExists(new[] { category1, category2 });
            Assert.AreEqual(2, categories.Length);
            Assert.AreEqual(category1, categories[0].Name);
            Assert.AreEqual(category2, categories[1].Name);

            var result = db.GetCategories();
            Assert.AreNotEqual(0, result.Length);
            Assert.IsTrue(result.Length >= 2);
        }

        [TestMethod]
        public void GetGateoriesCountersTest1()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();
            var result = db.GetGateoriesCounters();
        }


        [TestMethod]
        public void GetPostsInCategory1()
        {
            var db = TestsSetvices.Current.Get<MyBlog.Engine.DataService>();
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

            var result = db.GetPostsInCategory(categories[0].Id, 0);
            Assert.AreNotEqual(0, result.Length);
            Assert.IsTrue(result.Length >= 1);
        }
    }
}
