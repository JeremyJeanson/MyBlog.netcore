using MyBlog.Engine.Data.Models;
using System;

namespace MyBlog.Models
{
    public class Posts
    {
        public Int32 Page { get; set; }
        public Int32 NextPage { get; set; }
        public Int32 Available { get; set; }
        public Boolean HaveMoreResults { get; set; }
        public PostWithSummary[] Items { get; set; }
    }
}