using Microsoft.AspNetCore.Mvc;
using MyBlog.Engine.Services;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class ArchivesController : Controller
    {
        private readonly DataService _dataService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data"></param>
        public ArchivesController(DataService data)
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
        /// Index
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostIndex()
        {
            return PartialView("_Index", GetModel());
        }

        /// <summary>
        /// Get the model to use
        /// </summary>
        /// <returns></returns>
        private ArchivesViewModel GetModel()
        {
            // Get archives links
            var model = new ArchivesViewModel
            {
                Archives = _dataService.GetArchives()
            };
            return model;
        }
    }
}
