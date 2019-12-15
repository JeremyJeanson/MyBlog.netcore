using System;

namespace MyBlog.Engine.Models
{
    internal sealed class UserValidationMailData
    {
        internal String Name { get; set; }
        internal String Email { get; set; }
        internal Guid Token { get; set; }
    }
}
