using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Controllers
{
    public class SiteController : Controller
    {

        [HttpGet]
        public IActionResult ResourceNotFound()
        {
            return View("404");
        }

        [HttpGet("/about")]
        public IActionResult AboutPage()
        {
            ViewData["activeAbout"] = "show";
            ViewData["Title"] = "Sam Phal - About Me";
            return View("About");
        }

        [HttpGet("/portraits")]
        public IActionResult Portraits()
        {
            return View("Portraits");
        }

    }
}
