using Microsoft.AspNetCore.Mvc;
using MyBlog.Engine.Services;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DataService _dataService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data"></param>
        public CategoriesController(DataService data)
        {
            _dataService = data;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(GetModel());
        }

        /// <summary>
        /// Index for partial view
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostIndex()
        {
            return PartialView("_Index", GetModel());
        }

        /// <summary>
        /// Get the  model to use
        /// </summary>
        /// <returns></returns>
        private CategoriesVewModel GetModel()
        {
            var model = new CategoriesVewModel
            {
                Categories = _dataService.GetGateoriesCounters()
            };

            return model;
        }
    }
}
