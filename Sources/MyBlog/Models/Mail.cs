using MyBlog.Strings;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models
{
    public sealed class Mail
    {
        [Display(Name = "SenderName", ResourceType = typeof(Resources))]
        [Required]
        [MaxLength(100)]        
        public String SenderName { get; set; }

        [Display(Name = "SenderMail", ResourceType = typeof(Resources))]
        [Required]
        [EmailAddress]
        public String SenderMail { get; set; }

        [Display(Name = "Subject", ResourceType = typeof(Resources))]
        [Required]
        [MaxLength(200)]        
        public String Subject { get; set; }

        [Display(Name = "Message", ResourceType = typeof(Resources))]
        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(2000)]
        public String Content { get; set; }
    }
}