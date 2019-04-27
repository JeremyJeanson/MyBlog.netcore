using MyBlog.Engine.Data.Models;
using System;

namespace MyBlog.Models
{
    public class PostsFilter
    {
        public String Title { get; set; }
        public String SubTitle { get; set; }
        public String Description { get; set; }
        public String Action { get; set; }
        public String Id { get; set; }
        public Int32 Page { get; set; }
        public Int32 NextPage { get; set; }
        public Int32 Available { get; set; }
        public Boolean HaveMoreResults { get; set; }
        public PostWithoutContent[] Items { get; set; }
    }
}