using System;

namespace MyBlog.Engine.Data.Models
{
    public sealed class ArchiveLink
    {
        public ArchiveId Id { get; set; }
        public Int32 Count { get; set; }
        public String Title => Id?.ToString();
    }
}
