using System;
using System.Collections.Generic;

namespace MyBlog.Engine.Data.Models
{
    public abstract class PostDisplayedBase : IPost
    {
        public Int32 Id { get; set; }

        public String Title { get; set; }

        public DateTime DateCreatedGmt { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public Int32 CommentsCount { get; set; }

        public String Url { get; set; }
    }
}
