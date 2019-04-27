using System;

namespace MyBlog.Engine.Data.Models
{
    public interface IPost
    {
        Int32 Id { get; }
        String Title { get; }
    }
}
