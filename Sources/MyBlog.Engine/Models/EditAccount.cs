using MyBlog.Engine.Data.Models;
using System;

namespace MyBlog.Engine.Models
{
    public sealed class EditAccount
    {
        public UserProfile User { get; set; }

        public Nullable<Boolean> Success { get; set; }
    }
}
