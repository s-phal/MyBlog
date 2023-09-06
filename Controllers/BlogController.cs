using Microsoft.AspNetCore.Mvc;
using MyBlog.DataAccess;
using MyBlog.Models;
using X.PagedList;

namespace MyBlog.Controllers
{
    public class BlogController : Controller
    {

        private readonly BlogRepository _blogRepository;
        private readonly BlogCommentRepository _blogCommentRepository;
        private readonly BlogImageRepository _blogImageRepository;


        public BlogController(BlogRepository blogRepository, BlogCommentRepository blogComment, BlogImageRepository blogImageRepository)
        {
            _blogRepository = blogRepository;
            _blogCommentRepository = blogComment;
            _blogImageRepository = blogImageRepository;
        }


        // GET: BlogController
        public IActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var blog = _blogRepository.GetAllBlogsWithSummary();

            ViewData["Title"] = "Sam Phal - Code Snippets";
            return View(blog.ToPagedList(pageNumber, pageSize));
        }

        // GET: BlogController/Details/slug
        [HttpGet("/blog/{slug}")]
        public IActionResult Details(string slug)
        {
            BlogViewModel blogViewModel = new BlogViewModel();

            blogViewModel.Blog = _blogRepository.GetBlogDetailsBySlug(slug);

            if (blogViewModel.Blog == null)
            {
                return RedirectToAction("resourcenotfound", "site");
            }

            blogViewModel.BlogComments = _blogCommentRepository.GetBlogCommentsByBlogID(blogViewModel.Blog.blog_id);

            if (blogViewModel.BlogComments.Count() == 0)
            {
                ViewBag.Comments = 0;
            }

            blogViewModel.BlogCoverImage = _blogImageRepository.GetBlogCoverImageByBlogID(blogViewModel.Blog.blog_id);

            ViewData["Title"] = blogViewModel.Blog.title;
            ViewData["Summary"] = blogViewModel.Blog.summary;

            return View(blogViewModel);
        }

        [HttpGet("/search/{tagName}")]
        public IActionResult Search(int? page, string tagName)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            TempData["Search"] = tagName;

            var blog = _blogRepository.GetAllBlogsByTagName(tagName);

            return View("Search", blog.ToPagedList(pageNumber, pageSize));
        }





    }
}
