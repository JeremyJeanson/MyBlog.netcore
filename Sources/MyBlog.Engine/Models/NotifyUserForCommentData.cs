using System;

namespace MyBlog.Engine.Models
{
    internal sealed class NotifyUserForCommentData
    {
        internal String PostTitle { get; set; }
        internal String PostUri { get; set; }
        internal UserAddresse[] Users { get; set; }
    }
}
