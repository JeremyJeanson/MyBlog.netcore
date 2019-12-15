using System;

namespace MyBlog.Engine.Models
{
    public sealed class AuthenticationChoice
    {
        public String ReturnUrl { get; set; }
        public String Provider { get; set; }
    }
}
