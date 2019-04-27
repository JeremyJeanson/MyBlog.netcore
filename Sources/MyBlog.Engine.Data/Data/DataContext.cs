using Microsoft.EntityFrameworkCore;
using MyBlog.Engine.Data.Models;
using System;

namespace MyBlog.Engine.Data
{
    public sealed class DataContext: DbContext
    {
        #region Constructor

        public DataContext(DbContextOptions options) : base(options)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Utilisateurs XML RPC
        /// </summary>
        public DbSet<Publisher> Publishers { get; set; }

        /// <summary>
        /// Posts
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// Categories
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Users
        /// </summary>
        public DbSet<UserProfile> Users { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Posts to Categories (many to many)
        /// </summary>
        public DbSet<PostCategory> PostCategories { get; set; }

        /// <summary>
        /// UserProfile to Posts (many to many)
        /// </summary>
        public DbSet<UserProfilePost> UserProfilePosts { get; set; }


        #endregion

        #region Methodes

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Publishers - nothing
            // Posts - nothing       
            // Categories - nothing
            // Users - nothing

            // Comments
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.AuthorId);

            // PostCategories
            modelBuilder.Entity<PostCategory>()
                .HasKey(c => new { c.PostId, c.CategoryId });

            modelBuilder.Entity<PostCategory>()
                .HasOne(c => c.Post)
                .WithMany(c => c.Categories)
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<PostCategory>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(c => c.CategoryId);

            // UserProfilePosts
            modelBuilder.Entity<UserProfilePost>()
                .HasKey(c => new { c.UserProfileId, c.PostId });

            modelBuilder.Entity<UserProfilePost>()
                .HasOne(c => c.UserProfile)
                .WithMany(c => c.PostFollowed)
                .HasForeignKey(c => c.UserProfileId);

            modelBuilder.Entity<UserProfilePost>()
                .HasOne(c => c.Post)
                .WithMany(c => c.Followers)
                .HasForeignKey(c => c.PostId);
        }

        #endregion
    }
}
