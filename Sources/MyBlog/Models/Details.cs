using MyBlog.Engine.Data.Models;
using MyBlog.Strings;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models
{
    public sealed class Details
    {
        /// <summary>
        /// Meta description
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Post
        /// </summary>
        public PostWithDetails Post { get; set; }

        /// <summary>
        /// Comment model for edition
        /// </summary>
        public Comment Comment { get; set; }

        /// <summary>
        /// Previous post informations
        /// </summary>
        public PostLink PreviousPost { get; set; }

        /// <summary>
        /// Next post informations
        /// </summary>
        public PostLink NextPost { get; set; }

        /// <summary>
        /// User subscribed to comment notifications
        /// </summary>
        [Display(Name = "SubscribToCommentNotification",  ResourceType = typeof(Resources))]
        public Boolean CurrentUserSubscibed { get; set; }
    }
}