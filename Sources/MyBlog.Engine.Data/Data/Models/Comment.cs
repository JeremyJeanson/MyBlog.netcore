using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Engine.Data.Models
{
    public class Comment
    {
        #region Declarations
        #endregion

        #region Constructors
        #endregion

        #region Properties

        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        [MaxLength(2000)]
        [Required]
        [DataType(DataType.MultilineText)]
        public String Text { get; set; }

        public DateTime DateCreatedGmt { get; set; }

        [Column("Post_Id")]
        public Int32 PostId { get; set; }
        public Post Post { get; set; }

        [Column("Author_Id")]
        public Int32 AuthorId { get; set; }

        public UserProfile Author { get; set; }

        #endregion

        #region Methods
        #endregion
    }
}
