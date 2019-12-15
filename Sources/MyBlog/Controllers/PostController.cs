using MyBlog.Engine;
using MyBlog.Engine.Data.Models;
using MyBlog.Strings;
using MyBlog.Models;
using MyLib.Web.Helpers;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using MyBlog.Engine.Services;
using MyBlog.Engine.Models;

namespace MyBlog.Controllers
{
    public class PostController : Controller
    {
        #region Declarations

        private const String IndexGetMoreView = "_IndexGetMore";
        private const String FilterView = "Filter";
        private const String FilterGetMoreView = "_FilterGetMore";
        private const String SearchGetMoreView = "_SearchGetMore";
        private readonly DataService _dataService;
        private readonly IOptions<Settings> _options;
        private readonly UserService _userService;
        private readonly MailService _mailService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataService"></param>
        public PostController(DataService dataService, IOptions<Settings> options, UserService userService,MailService mailService)
        {
            _dataService = dataService;
            _options = options;
            _userService = userService;
            _mailService = mailService;
        }

        #endregion

        #region Posts

        /// <summary>
        /// Default Action (get posts) 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public ActionResult Index(Posts model)
        {
            // Initialize the model
            InitializePosts(model);
            return View(model);
        }

        public PartialViewResult IndexGetMore(Posts model)
        {
            // Initialize the model
            InitializePosts(model);
            return PartialView(IndexGetMoreView, model);
        }

        /// <summary>
        /// Initialize the model for posts
        /// </summary>
        /// <param name="model"></param>
        private void InitializePosts(Posts model)
        {
            // Get items
            model.Items = _dataService.GetPosts(model.Page * _options.Value.PostQuantityPerPage);

            // Get Counter to know if we have more items to load
            Int32 count = _dataService.Countposts();
            model.Available = count;
            model.HaveMoreResults = count > (model.Page + 1) * _options.Value.PostQuantityPerPage;

            // Update offset
            model.NextPage = model.Page + 1;
        }

        #endregion

        #region Categories

        /// <summary>
        /// Filter post by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Category(PostsFilter model)
        {
            // Filters have to share the id
            if (Int32.TryParse(model.Id, out int id))
            {
                // Title
                model.Title = Resources.Category;

                // Get name
                model.SubTitle = _dataService.GetCategoryName(id);
                model.Description = String.Format(
                    Resources.CategoryDescription,
                    model.SubTitle);

                // Initialize the data
                InitializeCategorymodel(model, id);
            }
            return View(FilterView, model);
        }

        /// <summary>
        /// Get more items when filter posts by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult CategoryGetMore(PostsFilter model)
        {
            // Filters have to share the id
            if (Int32.TryParse(model.Id, out int id))
            {
                InitializeCategorymodel(model, id);
            }
            return PartialView(FilterGetMoreView, model);
        }

        /// <summary>
        /// Add data on model used by category
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        private void InitializeCategorymodel(PostsFilter model, Int32 id)
        {
            // Action
            model.Action = "Category";

            // Gets posts
            model.Items = _dataService.GetPostsInCategory(id, model.Page * _options.Value.PostQuantityPerSearch);

            // Have more
            Int32 count = _dataService.CounPostsInCategory(id);
            model.Available = count;
            model.HaveMoreResults = count > (model.Page + 1) * _options.Value.PostQuantityPerSearch;

            // Update page index
            model.NextPage = model.Page + 1;
        }

        #endregion

        #region Details

        /// <summary>
        /// Get detail of post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(Int32 id)
        {
            // Initialize mode lwith post from database
            Details model = new Details()
            {
                Post = _dataService.GetPostWithDetails(id)
            };
            // Post not found
            if (model.Post == null)
            {
                return NotFound();
            }
            else
            {
                // Get description
                model.Description = WebpageHelper.GetMetaDescrition(model.Post.Summary);

                // Get previous and next posts informations, to display prevous and next buttons
                DateTime date = model.Post.DateCreatedGmt;

                model.PreviousPost = _dataService.GetPreviousPost(id, date);
                model.NextPost = _dataService.GetNextPost(id, date);

                // Build comment model to allow user to comment this post
                UserProfile user = _userService.Get();
                if (user == null)
                {
                    model.Comment = null;
                    model.CurrentUserSubscibed = false;
                }
                else
                {
                    model.Comment = new Comment();
                    model.CurrentUserSubscibed = _dataService.HasCurrentUserSubscibed(id, user.Id);
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Comment(Details model)
        {
            if (ModelState.IsValid)
            {
                // User id
                Int32 userId = _userService.Get()?.Id ?? 0;

                // Create the comment
                if(_dataService.AddComment(model.Comment.Text, userId, model.Post.Id))
                {
                    await _mailService.NotifyUsersForComment(model.Post.Id, userId);
                }
            }
            return RedirectToAction("Details", new { id = model.Post.Id });
        }

        [Authorize]
        [HttpPost]
        public PartialViewResult SubscribToCommentNotification(Int32 id, Boolean subscription)
        {
            Boolean model;
            // User id
            Int32 userId = _userService.Get()?.Id ?? 0;

            // Subscrib or unsubscribe
            model = _dataService.SubscribToCommentNotification(id, userId, subscription);
            return PartialView("_SubscribToCommentNotification", model);
        }

        #endregion

        #region Archives

        /// <summary>
        /// Filter post by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Archive(PostsFilter model)
        {
            ArchiveId id = new ArchiveId(model.Id);
            // Title
            model.Title = Resources.Archives;

            // Get name
            model.SubTitle = id.ToString();
            model.Description = String.Format(
                Resources.ArchivesDescription,
                model.SubTitle);

            // Initialize the data
            InitializeArchiveModel(model, id);

            return View(FilterView, model);
        }

        /// <summary>
        /// Get more items when filter posts by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult ArchiveGetMore(PostsFilter model)
        {
            ArchiveId id = new ArchiveId(model.Id);
            InitializeArchiveModel(model, id);
            return PartialView(FilterGetMoreView, model);
        }

        private void InitializeArchiveModel(PostsFilter model, ArchiveId id)
        {
            // Archive
            model.Action = "Archive";

            // Gets posts
            model.Items = _dataService.GetPostsInArchive(id, model.Page * _options.Value.PostQuantityPerSearch);

            // Have more
            Int32 count = _dataService.CounPostsInArchive(id);
            model.Available = count;
            model.HaveMoreResults = count > (model.Page + 1) * _options.Value.PostQuantityPerSearch;

            // Update page index
            model.NextPage = model.Page + 1;
        }

        #endregion

        #region Search

        /// <summary>
        /// Filter post by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Search(SearchFilter model)
        {
            // Title
            model.Title = Resources.Search;

            // Get name
            model.SubTitle = model.Query;
            model.Description = String.Format(
                Resources.SearchDescription,
                model.SubTitle);

            // Initialize the data
            InitializeSearchModel(model);
            return View(model);
        }

        /// <summary>
        /// Get more items when filter posts by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult SearchGetMore(SearchFilter model)
        {
            InitializeSearchModel(model);
            return PartialView(SearchGetMoreView, model);
        }

        /// <summary>
        /// Initialize a search model
        /// </summary>
        /// <param name="model"></param>
        private void InitializeSearchModel(SearchFilter model)
        {
            // Archive
            model.Action = "Search";

            if (String.IsNullOrWhiteSpace(model.Query))
            {
                model.Items = null;
                model.Available = 0;
                model.HaveMoreResults = false;
                model.NextPage = 0;
            }
            else
            {
                // Gets posts
                model.Items = _dataService.GetPostsInSearch(model.Query, model.Page * _options.Value.PostQuantityPerSearch);

                // Have more
                Int32 count = _dataService.CounPostsInSearch(model.Query);
                model.Available = count;
                model.HaveMoreResults = count > (model.Page + 1) * _options.Value.PostQuantityPerSearch;

                // Update page index
                model.NextPage = model.Page + 1;
            }
        }

        #endregion
    }
}