using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Engine.Data.Models
{
    public class Category
    {
        #region Déclarations

        #endregion

        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        [MaxLength(40)]
        public String Name { get; set; }

        public List<PostCategory> Posts { get; set; }

        #region Genenrated properties

        /// <summary>
        /// Url
        /// </summary>
        [NotMapped]
        public String Url { get; set; }

        #endregion
    }
}