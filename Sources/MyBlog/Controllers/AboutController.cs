using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ViewResult Index()
        {
            return View();
        }

        public  ViewResult PrivacyAndUsage()
        {
            return View();
        }

        public ViewResult Cookies()
        {
            return View();
        }
    }
}