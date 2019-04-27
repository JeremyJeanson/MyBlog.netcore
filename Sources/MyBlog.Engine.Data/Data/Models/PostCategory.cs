using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyBlog.Engine.Data.Models
{
    [Table("PostCategories")]
    public sealed class PostCategory
    {
        [Column("Post_Id")]
        public Int32 PostId { get; set; }

        public Post Post { get; set; }

        [Column("Category_Id")]
        public Int32 CategoryId { get; set; }

        public Category Category { get; set; }

    }
}
