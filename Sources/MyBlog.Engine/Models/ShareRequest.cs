using System;
using static MyBlog.Engine.TagHelpers.Socials;

namespace MyBlog.Engine.Models
{
    public sealed class ShareRequest
    {
        public String Id { get; set; }

        public SocialnetWork N { get; set; }
    }
}