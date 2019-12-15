using System;

namespace MyBlog.Engine.Models
{
    public sealed class AuthenticationSettings
    {
        public Boolean Active { get; set; }
        public String ClientId { get; set; }
        public String ClientSecret { get; set; }
    }
}