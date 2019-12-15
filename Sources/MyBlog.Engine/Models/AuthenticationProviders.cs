using System;

namespace MyBlog.Engine.Models
{
    public sealed class AuthenticationProviders
    {
        public String ReturnUrl { get; set; }
        public AuthenticationProvider[] Providers { get; set; }
    }
}
