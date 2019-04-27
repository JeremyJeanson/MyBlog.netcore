using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyBlog.Engine;
using MyLib.Web.SoeSiteMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBlog.Controllers
{
    public class SiteMapController : Controller
    {
        #region Declarations

        private readonly IOptions<Engine.Settings> _options;
        private readonly DataService _dataService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="dataService"></param>
        public SiteMapController(IOptions<Engine.Settings> options, DataService dataService)
        {
            _options = options;
            _dataService = dataService;
        }

        #endregion


        // GET: SiteMap
        public ActionResult Index()
        {
            SoeSiteMap sitemap = new SoeSiteMap(
                // Gt items
                GetItems().ToArray());

            return new SoeSiteMapResult(sitemap);
        }

        /// <summary>
        /// Returnn thee sitemap items for soe
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SoeSiteMapItem> GetItems()
        {
            String baseUri = _options.Value.Url+"/";

            // Home page
            yield return new SoeSiteMapItem(baseUri) { ChangeFrequence = Frequence.Daily, Priority = 1 };

            // About
            yield return new SoeSiteMapItem(baseUri + "About/");

            // All posts
            foreach(var link in _dataService.GetAllPostLink())
            {
                yield return new SoeSiteMapItem(link.Url) { Lastmodified = link.DatePublishedGmt, ChangeFrequence = Frequence.Yearly };
            }
        }
    }
}