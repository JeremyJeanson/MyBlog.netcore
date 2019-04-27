using System;

namespace MyBlog.Engine.Data.Models
{
    public class PostLinkWithDate
    {
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public DateTime DatePublishedGmt { get; set; }
        public String Url { get; set; }
    }
}
