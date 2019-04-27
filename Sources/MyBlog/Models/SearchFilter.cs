using System;

namespace MyBlog.Models
{
    public sealed class SearchFilter : PostsFilter
    {
        public String Query { get; set; }
    }
}