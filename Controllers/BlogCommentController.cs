using Microsoft.AspNetCore.Mvc;
using MyBlog.DataAccess;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class BlogCommentController : Controller
    {
        private readonly BlogCommentRepository _blogCommentRepository;

        public BlogCommentController(BlogCommentRepository blogCommentRepository)
        {
            _blogCommentRepository = blogCommentRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/blog/comment/create")]
        [ValidateAntiForgeryToken]
        public IActionResult BlogCommentCreate(BlogComment blogComment, Blog blog)
        {
            ModelState.Clear();
            TempData["DisplayCommentAlert"] = null;

            if (blogComment.content == null || !ModelState.IsValid)
            {
                return LocalRedirect("/blog/" + blog.slug);
            }

            if (blogComment.display_name == null)
            {
                blogComment.display_name = "Anonymous";
            }

            if (blogComment.user_website == null)
            {
                blogComment.user_website = "";
            }

            _blogCommentRepository.CreateBlogComment(blogComment);
            TempData["DisplayCommentAlert"] = "true";
            return LocalRedirect("/blog/" + blog.slug);

        }

    }
}
