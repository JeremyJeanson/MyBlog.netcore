using System;

namespace MyBlog.Engine.Models
{

    public sealed class AuthenticationCallback
    {
        public String ReturnUrl { get; set; }
        public String RemoteError { get; set; }
    }
}
