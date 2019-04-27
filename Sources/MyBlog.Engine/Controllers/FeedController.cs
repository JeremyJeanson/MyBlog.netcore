using Microsoft.AspNetCore.Mvc;
using MyLib.Web.Results;

namespace MyBlog.Engine.Controllers
{
    public class FeedController : Controller
    {
        #region Declarations

        private readonly FeedService _feedService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="feedService"></param>
        public FeedController(FeedService feedService)
        {
            _feedService = feedService;
        }

        #endregion
        
        #region Methodes

        /// <summary>
        /// Default format
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return new FeedResult(_feedService.Get(), FeedResult.Type.Rss);
        }

        /// <summary>
        /// Atom
        /// </summary>
        /// <returns></returns>
        public ActionResult Atom()
        {
            return new FeedResult(_feedService.Get(), FeedResult.Type.Atom);
        }

        /// <summary>
        /// Rss
        /// </summary>
        /// <returns></returns>
        public ActionResult Rss()
        {
            return new FeedResult(_feedService.Get(), FeedResult.Type.Rss);
        }

        #endregion
    }
}