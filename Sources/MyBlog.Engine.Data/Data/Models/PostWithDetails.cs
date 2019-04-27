using System;
using System.Collections.Generic;

namespace MyBlog.Engine.Data.Models
{
    public sealed class PostWithDetails: PostDisplayedBase
    {
        public String Summary { get; set; }

        public String Content { get; set; }

        public IEnumerable<CommentToDisplay> Comments { get; set; }

        public String HtmlFull
        {
            get
            {
                return Content;
            }
        }
    }
}
