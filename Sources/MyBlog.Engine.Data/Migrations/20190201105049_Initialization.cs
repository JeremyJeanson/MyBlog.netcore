using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBlog.Engine.Data.Migrations
{
    public partial class Initialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Categories",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Name = table.Column<string>(maxLength: 40, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Categories", x => x.Id);
            //    });
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
BEGIN
CREATE TABLE [Categories] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (40) NULL,
    CONSTRAINT [PK_dbo.Categories] PRIMARY KEY CLUSTERED ([Id] ASC)
)
END");

            //migrationBuilder.CreateTable(
            //    name: "Posts",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(maxLength: 150, nullable: true),
            //        BeginningOfContent = table.Column<string>(nullable: true),
            //        EndOfContent = table.Column<string>(nullable: true),
            //        ContentIsSplitted = table.Column<bool>(nullable: false),
            //        DateCreatedGmt = table.Column<DateTime>(nullable: false),
            //        Published = table.Column<bool>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Posts", x => x.Id);
            //    });
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Posts' AND xtype='U')
BEGIN
CREATE TABLE [Posts] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Title]              NVARCHAR (150) NULL,
    [BeginningOfContent] NVARCHAR (MAX) NULL,
    [EndOfContent]       NVARCHAR (MAX) NULL,
    [ContentIsSplitted]  BIT            NOT NULL,
    [DateCreatedGmt]     DATETIME       NOT NULL,
    [Published]          BIT            NOT NULL,
    CONSTRAINT [PK_dbo.Posts] PRIMARY KEY CLUSTERED ([Id] ASC)
)
END");

            //migrationBuilder.CreateTable(
            //    name: "Publishers",
            //    columns: table => new
            //    {
            //        Login = table.Column<string>(nullable: false),
            //        Password = table.Column<string>(nullable: true),
            //        Salt = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Publishers", x => x.Login);
            //    });
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Publishers' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Publishers] (
    [Login]    NVARCHAR (128) NOT NULL,
    [Password] NVARCHAR (MAX) NULL,
    [Salt]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Publishers] PRIMARY KEY CLUSTERED ([Login] ASC)
)
END");

            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Issuer = table.Column<string>(maxLength: 15, nullable: true),
            //        NameIdentifier = table.Column<string>(maxLength: 100, nullable: true),
            //        Name = table.Column<string>(maxLength: 50, nullable: false),
            //        Email = table.Column<string>(maxLength: 50, nullable: true),
            //        EmailValidate = table.Column<bool>(nullable: false),
            //        EmailValidationToken = table.Column<Guid>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.Id);
            //    });
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserProfiles' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[UserProfiles] (
    [Id]                   INT              IDENTITY (1, 1) NOT NULL,
    [Issuer]               NVARCHAR (15)    NULL,
    [NameIdentifier]       NVARCHAR (100)   NULL,
    [Name]                 NVARCHAR (50)    NOT NULL,
    [Email]                NVARCHAR (50)    NULL,
    [EmailValidate]        BIT              NOT NULL,
    [EmailValidationToken] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.UserProfiles] PRIMARY KEY CLUSTERED ([Id] ASC)
)
END");


            //migrationBuilder.CreateTable(
            //    name: "PostCategories",
            //    columns: table => new
            //    {
            //        Post_Id = table.Column<int>(nullable: false),
            //        Category_Id = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PostCategories", x => new { x.Post_Id, x.Category_Id });
            //        table.ForeignKey(
            //            name: "FK_PostCategories_Categories_Category_Id",
            //            column: x => x.Category_Id,
            //            principalTable: "Categories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_PostCategories_Posts_Post_Id",
            //            column: x => x.Post_Id,
            //            principalTable: "Posts",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PostCategories' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[PostCategories] (
    [Post_Id]     INT NOT NULL,
    [Category_Id] INT NOT NULL,
    CONSTRAINT [PK_dbo.PostCategories] PRIMARY KEY CLUSTERED ([Post_Id] ASC, [Category_Id] ASC),
    CONSTRAINT [FK_dbo.PostCategories_dbo.Posts_Post_Id] FOREIGN KEY ([Post_Id]) REFERENCES [dbo].[Posts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.PostCategories_dbo.Categories_Category_Id] FOREIGN KEY ([Category_Id]) REFERENCES [dbo].[Categories] ([Id]) ON DELETE CASCADE
)

CREATE NONCLUSTERED INDEX [IX_Post_Id]
    ON [dbo].[PostCategories]([Post_Id] ASC)

CREATE NONCLUSTERED INDEX [IX_Category_Id]
    ON [dbo].[PostCategories]([Category_Id] ASC)
END");


            //migrationBuilder.CreateTable(
            //    name: "Comments",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Text = table.Column<string>(maxLength: 2000, nullable: false),
            //        DateCreatedGmt = table.Column<DateTime>(nullable: false),
            //        Post_Id = table.Column<int>(nullable: false),
            //        Author_Id = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Comments", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Comments_Users_Author_Id",
            //            column: x => x.Author_Id,
            //            principalTable: "Users",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Comments_Posts_Post_Id",
            //            column: x => x.Post_Id,
            //            principalTable: "Posts",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Comments' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[Comments] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [Text]           NVARCHAR (2000) NOT NULL,
    [DateCreatedGmt] DATETIME        NOT NULL,
    [Author_Id]      INT             NULL,
    [Post_Id]        INT             NULL,
    CONSTRAINT [PK_dbo.Comments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Comments_dbo.UserProfiles_Author_Id] FOREIGN KEY ([Author_Id]) REFERENCES [dbo].[UserProfiles] ([Id]),
    CONSTRAINT [FK_dbo.Comments_dbo.Posts_Post_Id] FOREIGN KEY ([Post_Id]) REFERENCES [dbo].[Posts] ([Id])
)

CREATE NONCLUSTERED INDEX [IX_Author_Id]
    ON [dbo].[Comments]([Author_Id] ASC)

CREATE NONCLUSTERED INDEX [IX_Post_Id]
    ON [dbo].[Comments]([Post_Id] ASC)
END");


            //migrationBuilder.CreateTable(
            //    name: "UserProfilePosts",
            //    columns: table => new
            //    {
            //        UserProfile_Id = table.Column<int>(nullable: false),
            //        Post_Id = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserProfilePosts", x => new { x.UserProfile_Id, x.Post_Id });
            //        table.ForeignKey(
            //            name: "FK_UserProfilePosts_Posts_Post_Id",
            //            column: x => x.Post_Id,
            //            principalTable: "Posts",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_UserProfilePosts_Users_UserProfile_Id",
            //            column: x => x.UserProfile_Id,
            //            principalTable: "Users",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserProfilePosts' AND xtype='U')
BEGIN
CREATE TABLE [dbo].[UserProfilePosts] (
    [UserProfile_Id] INT NOT NULL,
    [Post_Id]        INT NOT NULL,
    CONSTRAINT [PK_dbo.UserProfilePosts] PRIMARY KEY CLUSTERED ([UserProfile_Id] ASC, [Post_Id] ASC),
    CONSTRAINT [FK_dbo.UserProfilePosts_dbo.UserProfiles_UserProfile_Id] FOREIGN KEY ([UserProfile_Id]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.UserProfilePosts_dbo.Posts_Post_Id] FOREIGN KEY ([Post_Id]) REFERENCES [dbo].[Posts] ([Id]) ON DELETE CASCADE
)

CREATE NONCLUSTERED INDEX [IX_UserProfile_Id]
    ON [dbo].[UserProfilePosts]([UserProfile_Id] ASC)

CREATE NONCLUSTERED INDEX [IX_Post_Id]
    ON [dbo].[UserProfilePosts]([Post_Id] ASC)
END");


            //migrationBuilder.CreateIndex(
            //    name: "IX_Comments_Author_Id",
            //    table: "Comments",
            //    column: "Author_Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Comments_Post_Id",
            //    table: "Comments",
            //    column: "Post_Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PostCategories_Category_Id",
            //    table: "PostCategories",
            //    column: "Category_Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserProfilePosts_Post_Id",
            //    table: "UserProfilePosts",
            //    column: "Post_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
