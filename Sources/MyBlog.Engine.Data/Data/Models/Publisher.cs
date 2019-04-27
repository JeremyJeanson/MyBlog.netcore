using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Engine.Data.Models
{
    public class Publisher
    {
        [Key]
        public String Login { get; set; }
        public String Password { get; set; }
        public String Salt { get; set; }
    }
}
