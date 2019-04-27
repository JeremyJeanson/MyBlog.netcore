using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Engine.Data.Models
{
    [Table("UserProfilePosts")]
    public sealed class UserProfilePost
    {
        [Column("UserProfile_Id")]
        public Int32 UserProfileId { get; set; }

        public UserProfile UserProfile { get; set; }


        [Column("Post_Id")]
        public Int32 PostId { get; set; }

        public Post Post { get; set; }
    }
}
