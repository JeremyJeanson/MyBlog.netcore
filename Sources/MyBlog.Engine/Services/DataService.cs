using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyBlog.Engine.Data;
using MyBlog.Engine.Data.Models;
using MyBlog.Engine.Models;
using MyLib.Web.Helpers;
using MyLib.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBlog.Engine.Services
{
    public sealed class DataService
    {

        #region Declarations

        private const String CategoriesPropertyName = "Categories";
        private const String CategoriesAndCategoryPropertyName = "Categories.Category";
        
        private const String PostUrlFormat = "{0}/Post/Details/{1}/{2}/";
        private const String CategoryUrlFormat = "{0}/Post/Category/{1}/{2}/";
        
        private readonly DataContext _context;
        private readonly IOptions<Settings> _options;

        #endregion

        #region Constructors

        public DataService(DataContext context, IOptions<Settings> options)
        {
            _context = context;
            _options = options;
        }

        #endregion

        #region Gestion des droits à utiliser XML RPC

        /// <summary>
        /// Test if publisher is allowed, create one if it is the first call (configuration, setup)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Boolean PublisherAccessAllowed(String login, String password)
        {
            // Test arguments
            if (String.IsNullOrWhiteSpace(login) 
                || String.IsNullOrWhiteSpace(password)) return false;

            // Test credentials
            Boolean result = TestPublisherCredentials(login, password);

            // All is ok
            if (result) return true;

            // Test if an user exists
            result = _context.Publishers.Any();

            // An user exists, creadentials are wrong
            if (result) return false;

            // This credentials are the very first to be used
            // Create an user with this credentials
            return CreatePublisher(login, password);
        }

        /// <summary>
        /// Test publisher credentials
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private Boolean TestPublisherCredentials(String login, String password)
        {
            // Get user to compare
            Publisher publisher = _context.Publishers.FirstOrDefault(c => c.Login.ToUpper() == login.ToUpper());

            // Publisher is available?
            if (publisher == null) return false;

            // Hash password to compare
            String hash = HashHelper.Hash(password, publisher.Salt);

            // Compare hashes
            return hash.Equals(publisher.Password);
        }

        /// <summary>
        /// Create a new publisher
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CreatePublisher(String login, String password)
        {
            // Get a new hash
            HashResult result = HashHelper.Hash(password);

            // Add publisher with hash
            _context.Publishers.Add(new Publisher
            {
                Login = login,
                Password = result.Hash,
                Salt = result.Salt
            });
            return _context.SaveChanges() > 0;
        }

        #endregion

        #region Categories

        /// <summary>
        /// Get categories ordered by name
        /// </summary>
        /// <returns></returns>
        public Category[] GetCategories()
        {
            var result = _context.Categories
                .OrderBy(c => c.Name)
                .ToArray();

            // Add urls
            foreach(var category in result)
            {
                category.Url = GetCategoryUrl(category.Id, category.Name);
            }
            return result;
        }

        /// <summary>
        /// Get naem of category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public String GetCategoryName(Int32 id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id)?.Name;
        }


        /// <summary>
        /// Get categories and creat if not exists
        /// </summary>
        /// <param name="categoriesNames"></param>
        /// <returns></returns>
        public Category[] GetCategoriesAndCreatIfNotExists(String[] categoriesNames)
        {
            return InnerGetCategoriesAndCreatIfNotExists(categoriesNames).ToArray();
        }

        /// <summary>
        /// Inner Get categories and creat if not exists
        /// </summary>
        /// <param name="categoriesNames"></param>
        /// <returns></returns>
        private IEnumerable<Category> InnerGetCategoriesAndCreatIfNotExists(String[] categoriesNames)
        {
            if (categoriesNames == null || categoriesNames.Length == 0) yield break;
            Category category;
            foreach(String categoryName in categoriesNames)
            {
                // Get category if exists
                category = _context.Categories.FirstOrDefault(c => c.Name == categoryName);
                // If this category donnot exist, create one
                if (category == null)
                {
                    category = new Category() { Name = categoryName };
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                }
                yield return category;
            }
        }

        /// <summary>
        /// Return list the lastests categories and count post of each
        /// </summary>
        /// <returns></returns>
        public Counter[] GetGateoriesCountersLatests()
        {
            return (from c in _context.Categories
                    where c.Posts.Any(p => p.Post != null && p.Post.Published && p.Post.DateCreatedGmt <= DateTime.UtcNow)
                    select new Counter() { Id = c.Id, Name = c.Name, Count = c.Posts.Count(p => p.Post != null && p.Post.Published && p.Post.DateCreatedGmt <= DateTime.UtcNow) }
                    )
                    .OrderByDescending(c => c.Count)
                    .Take(Constants.CategoriesDisplayedOnLayoutMax)
                    .ToArray();
        }

        /// <summary>
        /// Return list of all ccategories and count post of each
        /// </summary>
        /// <returns></returns>
        public Counter[] GetGateoriesCounters()
        {
            return (from c in _context.Categories
                    where c.Posts.Any(p => p.Post != null && p.Post.Published && p.Post.DateCreatedGmt <= DateTime.UtcNow)
                    select new Counter() { Id = c.Id, Name = c.Name, Count = c.Posts.Count(p => p.Post != null && p.Post.Published && p.Post.DateCreatedGmt <= DateTime.UtcNow) }
                    ).OrderByDescending(c => c.Count).ToArray();
        }

        /// <summary>
        /// Add a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Boolean AddCategory(Category category)
        {
            _context.Categories.Add(category);
            return _context.SaveChanges() > 0;
        }

        /// <summary>
        /// Remove/Delete category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean DeleteCategory(Int32 id)
        {
            Category category = new Category { Id = id };
            _context.Categories.Attach(category);
            _context.Entry(category).State = EntityState.Deleted;
            return _context.SaveChanges() > 0;
        }

        /// <summary>
        /// Edit a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Boolean EditCategory(Category category)
        {
            _context.Categories.Attach(category);
            var entry = _context.Entry(category);
            entry.State = EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        /// <summary>
        /// Remove a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Boolean RemoveCategory(Category category)
        {
            _context.Categories.Remove(category);
            return _context.SaveChanges() > 0;
        }

        #endregion

        #region Posts

        /// <summary>
        /// Get url for a post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public String GetPostUrl(IPost post)
        {
            return GetPostUrl(post.Id, post.Title);
        }

        /// <summary>
        /// Get url for a post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public String GetPostUrl(Int32 id, String title)
        {
            return String.Format(PostUrlFormat,
                _options.Value.Url,
                id.ToString(),
                UriHelper.ToFriendly(title));
        }

        /// <summary>
        /// Return Url for category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public String GetCategoryUrl(int id, string name)
        {
            return String.Format(
                CategoryUrlFormat,
                _options.Value.Url,
                id.ToString(),
                UriHelper.ToFriendly(name));
        }

        /// <summary>
        /// Get all  post links for SEO sitemap
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PostLinkWithDate> GetAllPostLink()
        {
            return _context.Posts.Where(c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow)
                .Select(c => new 
                {
                    c.Id,
                    c.Title,
                    c.DateCreatedGmt
                }).AsEnumerable()
                .Select(c => new PostLinkWithDate
                {
                    Id = c.Id,
                    Title = c.Title,
                    DatePublishedGmt = c.DateCreatedGmt,
                    Url = GetPostUrl(c.Id, c.Title)
                })
                // Enumerate the content to avoid  lazy load when datacontext is closed
                .AsEnumerable();
        }

        /// <summary>
        /// Count posts available
        /// </summary>
        /// <returns></returns>
        public Int32 Countposts()
        {
            return _context.Posts.Count(c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow);
        }

        public Int32 CountpostsAndDrafts()
        {
            return _context.Posts.Count();
        }

        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Post GetPost(Int32 id)
        {
            // Get data
            Post result = _context.Posts
                .Include(CategoriesPropertyName)
                .Include(CategoriesAndCategoryPropertyName)
                .FirstOrDefault(c => c.Id == id);
            if (result == null) return null;

            // Add url
            result.Url = GetPostUrl(result.Id, result.Title);
            return result;
        }

        /// <summary>
        /// Get post by id, with full details and comments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PostWithDetails GetPostWithDetails(Int32 id)
        {
            return _context.Posts
                .Include(CategoriesPropertyName)
                .Include(CategoriesAndCategoryPropertyName)
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.DateCreatedGmt,
                    Summary = c.BeginningOfContent,
                    Content = c.ContentIsSplitted
                        ? c.BeginningOfContent + c.EndOfContent
                        : c.BeginningOfContent,
                    Categories = c.Categories.Select(cat => cat.Category),
                    CommentsCount = c.Comments.Count,
                    Comments = c.Comments.Select(
                        cc => new CommentToDisplay
                        {
                            Author = cc.Author.Name,
                            Text = cc.Text,
                            DateCreatedGmt = cc.DateCreatedGmt
                        }
                        )
                })
                .AsEnumerable()
                .Select(c => new PostWithDetails
                {
                    Id = c.Id,
                    Title = c.Title,
                    DateCreatedGmt = c.DateCreatedGmt,
                    Url = GetPostUrl(c.Id, c.Title),
                    Summary = c.Summary,
                    Content = c.Content,
                    Categories = c.Categories,
                    CommentsCount = c.CommentsCount,
                    Comments = c.Comments
                }).FirstOrDefault();
        }

        /// <summary>
        /// Get link to a post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PostLink GetPostLink(Int32 id)
        {
            // Get data
            var result = _context.Posts
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                }).FirstOrDefault();

            // Test the result
            if (result == null) return null;

            // Return a formated data
            return new PostLink
            {
                Id = result.Id,
                Title = result.Title,
                Url = GetPostUrl(result.Id, result.Title)
            };                
        }


        /// <summary>
        /// Get latest posts 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public PostWithSummary[] GetPosts(Int32 offset)
        {
            return GetPosts(offset, _options.Value.PostQuantityPerPage);
        }

        /// <summary>
        /// Get latest posts 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public PostWithSummary[] GetPosts(Int32 offset,Int32 count)
        {
            return _context.Posts
                .Include(CategoriesPropertyName)
                .Where(c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow)
                .OrderByDescending(c => c.DateCreatedGmt)
                .Skip(offset)
                .Take(count)
                .Select(c => new
                {
                    c.Id,
                    Categories=c.Categories.Select(cat=>cat.Category),
                    c.ContentIsSplitted,
                    c.DateCreatedGmt,
                    c.BeginningOfContent,
                    c.Title,
                    CommentsCount = c.Comments.Count
                })
                .AsEnumerable()
                .Select(c => new PostWithSummary
                {
                    Id = c.Id,
                    Categories = c.Categories,
                    ContentIsSplitted = c.ContentIsSplitted,
                    DateCreatedGmt = c.DateCreatedGmt,
                    Url = GetPostUrl(c.Id, c.Title),
                    Summary = c.BeginningOfContent,
                    Title = c.Title,
                    CommentsCount = c.CommentsCount
                })
                .ToArray();
        }

        public Post[] GetPostsAndDrafts(Int32 offset, Int32 count)
        {
            return _context.Posts
                .Include(CategoriesPropertyName)
                .Include(CategoriesAndCategoryPropertyName)
                .OrderByDescending(c => c.DateCreatedGmt)
                .Skip(offset)
                .Take(count)
                .ToArray();
        }

        /// <summary>
        /// Add a new post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public Boolean AddPost(Post post, Int32[] categories)
        {
            // Add
            _context.Posts.Add(post);
            
            // Save
            if (_context.SaveChanges() > 0)
            {
                // Test categories
                if (categories?.Length <= 0)
                {
                    return true;
                }
                
                // Add categories
                for (Int32 i = 0; i < categories.Length; i++)
                {
                    _context.PostCategories.Add(
                        new PostCategory
                        {
                            PostId = post.Id,
                            CategoryId = categories[i]
                        });
                }

                // Save
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public Boolean DeletePost(Int32 id)
        {
            // Test if entity is not in EF cache, then detach this cache
            var local = _context.Set<Post>()
                .Local.FirstOrDefault(c => c.Id == id);
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            // Get the post to delete
            Post post = new Post { Id = id };
            _context.Posts.Attach(post);
            _context.Entry(post).State = EntityState.Deleted;
            return _context.SaveChanges() > 0;
        }

        public Boolean EditPost(Post post, Int32[] categories)
        {
            // Test if entity is not in EF cache, then detach this cache
            var local = _context.Set<Post>()
                .Local.FirstOrDefault(c => c.Id == post.Id);
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            // Get categories to have
            post.Categories?.Clear();

            // Sage the post
            _context.Posts.Attach(post);
            _context.Entry(post).State = EntityState.Modified;
            Boolean saved = _context.SaveChanges() > 0;

            // Get categories stored
            PostCategory[] categriesStored = _context.PostCategories
                .Where(c => c.PostId == post.Id)
                .ToArray();

            // Rmove categories
            foreach (var category in categriesStored)
            {
                _context.PostCategories.Remove(category);
            }
            _context.SaveChanges();

            // Add categories
            if (categories?.Length > 0)
            {
                for (Int32 i = 0; i < categories.Length; i++)
                {
                    _context.PostCategories.Add(
                        new PostCategory
                        {
                            PostId = post.Id,
                            CategoryId = categories[i]
                        });
                }
                // Save
                return _context.SaveChanges() > 0;
            }

            return saved;
        }

        /// <summary>
        /// Get link to previous post
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public PostLink GetPreviousPost(Int32 postId, DateTime date)
        {
            var result = (from c in _context.Posts
                          where c.Published && c.DateCreatedGmt < DateTime.UtcNow && c.Id != postId && c.DateCreatedGmt < date
                          orderby c.DateCreatedGmt descending
                          select new { c.Id, c.Title }
                    ).FirstOrDefault();
            
            // Test if data are available
            if (result == null) return null;
            
            // format the result
            return new PostLink
            {
                Id = result.Id,
                Title = result.Title,
                Url = GetPostUrl(result.Id, result.Title)
            };
        }

        /// <summary>
        ///  Get link to next post
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public PostLink GetNextPost(Int32 postId, DateTime date)
        {
            var result = (from c in _context.Posts
                          where c.Published && c.DateCreatedGmt < DateTime.UtcNow && c.Id != postId && c.DateCreatedGmt > date
                          orderby c.DateCreatedGmt ascending
                          select new { c.Id, c.Title }
                    ).FirstOrDefault();

            // Test if data are available
            if (result == null) return null;

            // format the result
            return new PostLink
            {
                Id = result.Id,
                Title = result.Title,
                Url = GetPostUrl(result.Id, result.Title)
            };
        }

        #endregion

        #region Archives

        /// <summary>
        /// Get only latest archives
        /// </summary>
        /// <returns></returns>
        public ArchiveLink[] GetArchivesLatests()
        {
            return (from p in _context.Posts
                         where p.Published && p.DateCreatedGmt <= DateTime.UtcNow
                         group p by new { p.DateCreatedGmt.Year, p.DateCreatedGmt.Month } into g
                         select new
                         {
                             g.Key.Year,
                             g.Key.Month,
                             Count = g.Count()
                         })
                         .OrderByDescending(c=>c.Year)
                         .ThenByDescending(c=>c.Month)
                         .Take(Constants.ArchivesDisplayedOnLayoutMax)
                         .AsEnumerable()
                         .Select(m => new ArchiveLink
                         {
                             Id = new ArchiveId(m.Year, m.Month),
                             Count = m.Count
                         })
                         .ToArray();
        }

        /// <summary>
        /// Get all archives links and counters
        /// </summary>
        /// <returns></returns>
        public ArchiveLink[] GetArchives()
        {
            var years = (from p in _context.Posts
                         where p.Published && p.DateCreatedGmt <= DateTime.UtcNow
                         group p by new { p.DateCreatedGmt.Year, p.DateCreatedGmt.Month } into g
                         select new
                         {
                             g.Key.Year,
                             g.Key.Month,
                             Count = g.Count()
                         })
                         .AsEnumerable()
                       .GroupBy(c => c.Year)
                       .OrderByDescending(c=>c.Key)
                    .ToArray();

            if (years.Count() > 2)
            {
                return years.Take(2).SelectMany(y =>
                    y.OrderByDescending(m => m.Month).Select(m =>
                        new ArchiveLink
                        {
                            Id = new ArchiveId(m.Year, m.Month),
                            Count = m.Count
                        }))
                        .Union(
                        years.Skip(2).Select(y =>
                        new ArchiveLink
                        {
                            Id = new ArchiveId(y.Key),
                            Count = y.Sum(m => m.Count)
                        })
                        )
                        .ToArray();
            }
            else
            {
                return years
                    .SelectMany(y => y.OrderByDescending(m => m.Month)
                    .Select(m => new ArchiveLink
                    {
                        Id = new ArchiveId(m.Year, m.Month),
                        Count = m.Count
                    })).ToArray();
            }
        }

        //public Post Get
        /// <summary>
        /// Get posts in categroy
        /// </summary>
        /// <param name="archiveId"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public PostWithoutContent[] GetPostsInArchive(ArchiveId id, Int32 offset)
        {
            Func<Post, Boolean> predicate;

            if (id.Month.HasValue)
            {
                predicate = c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow
                    && c.DateCreatedGmt.Year == id.Year && c.DateCreatedGmt.Month == id.Month;
            }
            else
            {
                predicate = c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow
                    && c.DateCreatedGmt.Year == id.Year;
            }

            return _context.Posts
                .Include(CategoriesPropertyName)
                .Include(CategoriesAndCategoryPropertyName)
                .Where(predicate)
                .OrderByDescending(c => c.DateCreatedGmt)
                .Skip(offset)
                .Take(_options.Value.PostQuantityPerSearch)
                .Select(c =>
                    new PostWithoutContent
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Categories = c.Categories.Select(cat => cat.Category),
                        DateCreatedGmt = c.DateCreatedGmt,
                        Url = GetPostUrl(c.Id, c.Title)
                    }
                ).ToArray();
        }

        /// <summary>
        /// Count posts in category
        /// </summary>
        /// <param name="archiveId"></param>
        /// <returns></returns>
        public Int32 CounPostsInArchive(ArchiveId id)
        {
            Func<Post, Boolean> predicate;

            if (id.Month.HasValue)
            {
                predicate = c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow
                     && c.DateCreatedGmt.Year == id.Year && c.DateCreatedGmt.Month == id.Month;
            }
            else
            {
                predicate = c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow
                    && c.DateCreatedGmt.Year == id.Year;
            }


            return _context.Posts
                .Count(predicate);
        }

        #endregion

        #region Filter on category

        /// <summary>
        /// Get posts in categroy
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public PostWithoutContent[] GetPostsInCategory(Int32 categoryId, Int32 offset)
        {
            return _context.PostCategories
                .Where(p => p.CategoryId == categoryId && p.Post != null && p.Post.Published && p.Post.DateCreatedGmt <= DateTime.UtcNow)
                .OrderByDescending(c => c.Post.DateCreatedGmt)
                .Skip(offset)
                .Take(_options.Value.PostQuantityPerSearch)
                .Select(c=> new {
                    c.Post.Id,
                    c.Post.Title,
                    c.Post.DateCreatedGmt,
                    Categories = c.Post.Categories.Select(cc=>cc.Category) })
                .AsEnumerable()
                .Select(c =>
                    new PostWithoutContent
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Categories = c.Categories,
                        DateCreatedGmt = c.DateCreatedGmt,
                        Url = GetPostUrl(c.Id, c.Title)
                    }
                ).ToArray();
        }

        /// <summary>
        /// Count posts in category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Int32  CounPostsInCategory(Int32 categoryId)
        {
            return _context.PostCategories
                .Count(c => c.CategoryId == categoryId);
        }

        #endregion

        #region Search

        /// <summary>
        /// Get posts in Search
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public PostWithoutContent[] GetPostsInSearch(String query, Int32 offset)
        {
            return _context.Posts
                .Include(CategoriesPropertyName)
                .Where(c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow
                    && (
                        c.Title.Contains(query)
                        || c.EndOfContent.Contains(query)
                    )
                )
                .OrderByDescending(c => c.DateCreatedGmt)
                .Skip(offset)
                .Take(_options.Value.PostQuantityPerSearch)
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    Categories = c.Categories.Select(cat => cat.Category),
                    c.DateCreatedGmt,
                })
                .AsEnumerable()
                .Select(c =>
                    new PostWithoutContent
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Categories = c.Categories,
                        DateCreatedGmt = c.DateCreatedGmt,
                        Url = GetPostUrl(c.Id, c.Title)
                    }
                ).ToArray();
        }

        /// <summary>
        ///  Count posts in Search
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Int32 CounPostsInSearch(String query)
        {
            return _context.Posts
                .Count(c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow
                    && (
                        c.Title.Contains(query)
                        || c.EndOfContent.Contains(query)
                    )
                );
        }

        #endregion

        #region Users

        /// <summary>
        /// Get user from claims information
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="nameIdentifier"></param>
        /// <returns></returns>
        internal UserProfile GetUser(String issuer, String nameIdentifier)
        {
            return _context.Users.FirstOrDefault(c =>
                c.Issuer == issuer
                && c.NameIdentifier == nameIdentifier);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal UserProfile GetUser(Int32 userId)
        {
            return _context.Users.FirstOrDefault(c =>
                c.Id == userId);
        }

        /// <summary>
        /// Edit an user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public EditUserData EditUser(UserProfile user)
        {
            // test user
            if (user == null) return null;

            EditUserData result = new EditUserData();

            // Test insert or update
            if (user.Id == 0)
            {
                result.MailChanged = true;
                user.EmailValidate = false;
                user.EmailValidationToken = Guid.NewGuid();

                // Insert
                _context.Users.Add(user);
            }
            else
            {
                // Get current mail to know if it was changed
                String email = _context.Users.Where(c => c.Id == user.Id)
                    .Select(c => c.Email)
                    .FirstOrDefault();
                result.MailChanged = String.Compare(email, user.Email, StringComparison.InvariantCultureIgnoreCase) != 0;

                // Attach data for update
                _context.Users.Attach(user);
                // Get entry
                var entry = _context.Entry(user);
                entry.State = EntityState.Unchanged;
                entry.Property(c => c.Name).IsModified = true;
                entry.Property(c => c.Email).IsModified = true;
                
                // Test if mail changed
                if (result.MailChanged)
                {
                    user.EmailValidate = false;
                    user.EmailValidationToken = Guid.NewGuid();

                    entry.Property(c => c.EmailValidate).IsModified = true;
                    entry.Property(c => c.EmailValidationToken).IsModified = true;
                }
            }


            // Save
            if (_context.SaveChanges() <= 0) return null;
            if (String.IsNullOrWhiteSpace(user.Email))
            {
                // impossible to have to send email because this user have none email addresse
                result.MailChanged = false;
            }

            // Result is ok
            result.Result = true;
            return result;

            // TODO : If mail changed, send validation mail
            //  SendValidationMail(user.Id, user.Name, user.Email, user.EmailValidationToken);
        }

        public sealed class EditUserData
        {
            public Boolean Result { get; set; }
            public Boolean MailChanged { get; set; }
        }

        #endregion

        #region Comments

        /// <summary>
        /// Add comment
        /// </summary>
        /// <param name="text"></param>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public Boolean AddComment(String text, Int32 userId, Int32 postId)
        {
            // Get entry for post
            Post post = new Post { Id = postId };
            _context.Posts.Attach(post);

            // Get entry for author
            UserProfile user = new UserProfile { Id = userId };
            _context.Users.Attach(user);

            // Create a new comment
            Comment comment = new Comment
            {
                Text = text.Replace(Environment.NewLine,"<br/>"),
                DateCreatedGmt = DateTime.UtcNow,
                Post = post,
                Author = user
            };

            // Add comment to databas
            _context.Comments.Add(comment);
            //if (_context.SaveChanges() > 0)
            //{
            //    await NotifyUserForComment(postId, userId);
            //    return true;
            //}
            //return false;
            // TODO : When you call this methode, use MailService to notify users
            return _context.SaveChanges() > 0;
        }

        /// <summary>
        /// Send mail on comments subsribers when a comment has been added
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="authorOfComment"></param>
        /// <returns></returns>
        internal NotifyUserForCommentData GetNotifyUserForCommentData(Int32 postId, Int32 authorOfComment)
        {
            String authorMail = _options.Value.AuthorMail;
            String authorName = _options.Value.AuthorName;

            // Get post informations
            var post = (from p in _context.Posts
                        where p.Id == postId
                        select new
                        {
                            p.Title,
                            // Take only other users with mail validated
                            Users = (from u in p.Followers
                                     where u.UserProfile !=null &&  u.UserProfile.Id != authorOfComment && u.UserProfile.EmailValidate && !String.IsNullOrEmpty(u.UserProfile.Email) && u.UserProfile.Email != authorMail
                                     group u by u.UserProfile.Email into g
                                     let c = g.FirstOrDefault()
                                     select new { c.UserProfile.Name, c.UserProfile.Email })
                        }
                        ).FirstOrDefault();

            // data null
            if (post == null) return null;

            // Format data
            return new NotifyUserForCommentData
            {
                PostTitle = post.Title,
                PostUri = GetPostUrl(postId, post.Title),
                Users = post.Users?.Select(c=>new UserAddresse
                    {
                        Email = c.Email,
                        Name = c.Name
                    }).ToArray()
            };
        }

        /// <summary>
        /// Subscrib or unsubscrib
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <param name="subscribtion"></param>
        /// <returns></returns>
        public Boolean SubscribToCommentNotification(Int32 postId, Int32 userId, Boolean subscribtion)
        {
            Boolean post = _context.Posts
                .Any(c => c.Id == postId);
            if (!post) return false;

            // Try to know if user as allways subscrib
            UserProfilePost link = _context.UserProfilePosts.FirstOrDefault(c =>
                c.UserProfileId == userId
                && c.PostId == postId);

            if (link == null)
            {
                // Susbsctrib
                _context.UserProfilePosts.Add(new UserProfilePost
                {
                    UserProfileId = userId,
                    PostId = postId
                });

                return _context.SaveChanges() > 0;
            }
            else
            {
                // Unsubscrib
                _context.UserProfilePosts.Remove(link);

                return _context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Know if current user has subscribed to this post comments notifications
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean HasCurrentUserSubscibed(Int32 postId, Int32 userId)
        {
            return _context.UserProfilePosts.Any(c => c.PostId == postId && c.UserProfileId == userId);
        }

        #endregion

        #region Mail validation

        /// <summary>
        /// Validate an email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="validationKey"></param>
        /// <returns></returns>
        public Boolean ValidateMail(Int32 userId, Guid validationToken)
        {
            Boolean ok = _context.Users.Any(c => c.Id == userId && c.EmailValidationToken == validationToken);
            if (ok)
            {
                // User to update
                UserProfile user = new UserProfile
                {
                    Id = userId,
                    EmailValidate = true
                };

                // Test EF local cache
                var local = _context.Set<UserProfile>()
                    .Local
                    .FirstOrDefault(c => c.Id == user.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }

                // Attach and mark property that was modified
                _context.Users.Attach(user);
                var entry = _context.Entry(user);
                entry.State = EntityState.Unchanged;
                entry.Property(c => c.EmailValidate).IsModified = true;

                // do not chech others model data
                //_context.Configuration.ValidateOnSaveEnabled = false; // (old EF6 code)
                Boolean saved = _context.SaveChanges() > 0;
                //_context.Configuration.ValidateOnSaveEnabled = true;

                // Save
                return saved;
            }
            return false;
        }

        /// <summary>
        /// Retourne une token pour envoi par mail
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal UserValidationMailData GetSendValidationMailToken(Int32 userId)
        {
            // Get data from this user
            var userData = (from u in _context.Users
                            where u.Id == userId
                            select new { u.Name, u.Email }).FirstOrDefault();
            if (userData == null || String.IsNullOrEmpty(userData.Email)) return null;

            Guid token = Guid.NewGuid();

            // User to update
            UserProfile user = new UserProfile
            {
                Id = userId,
                EmailValidate = false,
                EmailValidationToken = token
            };

            // Test if entity is not in EF cache, then detach this cache
            var local = _context.Set<UserProfile>()
                .Local.FirstOrDefault(c => c.Id == user.Id);
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            // Attach and mark property that was modified
            _context.Users.Attach(user);
            var entry = _context.Entry(user);
            entry.State = EntityState.Unchanged;
            entry.Property(c => c.EmailValidate).IsModified = true;
            entry.Property(c => c.EmailValidationToken).IsModified = true;

            // do not chech others model data
            //_context.Configuration.ValidateOnSaveEnabled = false; // (old EF6 code)
            Boolean saved = _context.SaveChanges() > 0;
            //_context.Configuration.ValidateOnSaveEnabled = true;

            // Return data if database was saved
            if (saved)
            {
                return new UserValidationMailData
                {
                    Email = userData.Email,
                    Name = userData.Name,
                    Token = token
                };
            }
            return null;
        }

        #endregion
    }
}
