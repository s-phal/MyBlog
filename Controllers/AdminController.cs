using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Core;
using MyBlog.DataAccess;
using MyBlog.Models;
using System.Security.Claims;

namespace MyBlog.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly BlogRepository _blogRepository;
        private readonly BlogTagRepository _blogTagRepository;
        private readonly BlogCommentRepository _blogCommentRepository;
        private readonly BlogImageRepository _blogImageRepository;
        private readonly BlogUserRepository _blogUserRepository;

        public AdminController(BlogRepository blogRepository, BlogTagRepository blogTagRepository, BlogCommentRepository blogCommentRepository, BlogImageRepository blogImageRepository, BlogUserRepository blogUserRepository)
        {
            _blogRepository = blogRepository;
            _blogTagRepository = blogTagRepository;
            _blogCommentRepository = blogCommentRepository;
            _blogImageRepository = blogImageRepository;
            _blogUserRepository = blogUserRepository;
        }


        public IActionResult Index()
        {
            AdminViewModel adminViewModel = new AdminViewModel();

            adminViewModel.Blogs = _blogRepository.GetAllBlogsForAdminDataTable();
            adminViewModel.BlogComments = _blogCommentRepository.GetAllBlogCommentsForAdminCommentTable();

            return View("Dashboard", adminViewModel);
        }

        [HttpGet("/admin/blog/create")]
        public IActionResult BlogCreate()
        {
            return View("BlogCreate");
        }

        [HttpPost("/admin/blog/create")]
        [ValidateAntiForgeryToken]
        public IActionResult BlogCreate(Blog blog, BlogTag blogTag, IFormFile uploadedFile)
        {

            if (!ModelState.IsValid)
            {
                return View("BlogCreate", blog);
            }

            var newBlog = _blogRepository.CreateBlog(blog);

            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                string uniqueFileName = _blogImageRepository.UploadImage(uploadedFile);
                _blogImageRepository.CreateBlogCoverImage(newBlog.blog_id, uniqueFileName);
            }

            if (blogTag.name != null)
            {
                List<string> tagsList = CommonLib.ConvertStringToWordList(blogTag.name);
                List<string> tags = CommonLib.RemoveDuplicateWordsFromList(tagsList);

                foreach (var tag in tags)
                {
                    if (!string.IsNullOrEmpty(tag))
                    {
                        _blogTagRepository.CreateTag(newBlog.blog_id, tag);
                    }
                }

            }

            return Redirect("/blog/" + blog.slug);

        }

        [HttpGet("/admin/blog/edit/{blogID}")]
        public ActionResult BlogUpdate(int blogID)
        {
            if (!CommonLib.IsValidNonZeroNumber(blogID))
            {
                return RedirectToAction("Index");
            }

            BlogViewModel blogViewModel = new BlogViewModel();

            blogViewModel.Blog = _blogRepository.GetBlogDetailsByID(blogID);
            blogViewModel.BlogTags = _blogTagRepository.GetAllBlogTagsByBlogID(blogID);

            return View("BlogUpdate", blogViewModel);

        }

        [HttpPost("/admin/blog/edit/{blogID}")]
        [ValidateAntiForgeryToken]
        public ActionResult BlogUpdate(Blog blog, BlogTag blogTag)
        {

            if (blog.title == null || blog.content == null)
            {
                TempData["Message"] = "Process failed!";
                return RedirectToAction("Index");
            }

            _blogRepository.UpdateBlog(blog);

            if (blogTag.name != null)
            {
                List<string> tagsList = CommonLib.ConvertStringToWordList(blogTag.name);
                List<string> tags = CommonLib.RemoveDuplicateWordsFromList(tagsList);

                foreach (var tag in tags)
                {
                    if (!string.IsNullOrEmpty(tag))
                    {
                        _blogTagRepository.CreateTag(blog.blog_id, tag);
                    }
                }
            }

            return Redirect("/blog/" + blog.slug);

        }

        [HttpPost("/admin/blog/delete/{blogID}")]
        [ValidateAntiForgeryToken]
        public IActionResult BlogDelete(int blogID)
        {

            _blogTagRepository.DeleteAllBlogTagsByBlogID(blogID);
            _blogCommentRepository.DeleteAllBlogCommentsByBlogID(blogID);

            _blogRepository.DeleteBlog(blogID);

            TempData["Message"] = "Blog-ID " + blogID + " Deleted!";

            return RedirectToAction("Index");

        }

        [HttpGet("/admin/blogtag/delete/{blogTagID}/{blogID}")]
        public IActionResult BlogTagDelete(int blogTagID, int blogID)
        {

            _blogTagRepository.DeleteBlogTag(blogTagID);

            return LocalRedirect("/admin/blog/edit/" + blogID);

        }

        [HttpPost("/admin/comment/manage/{approveFlag}/{blogCommentID}")]
        public IActionResult BlogCommentManage(int approveFlag, int blogCommentID)
        {
            if (approveFlag == 1)
            {
                _blogCommentRepository.ApproveBlogCommentByCommentID(blogCommentID);

                return RedirectToAction("Index");
            }

            _blogCommentRepository.DeleteBlogCommentByCommentID(blogCommentID);

            return RedirectToAction("Index");

        }

        [AllowAnonymous]
        [HttpGet("/admin/signin")]
        public IActionResult SignIn()
        {
            return View("SignIn");

        }

        [AllowAnonymous]
        [HttpPost("/admin/signin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(string username, string password)
        {
            var passwordHasher = new PasswordHasher<string>();

            var blogUser = _blogUserRepository.GetStoredPasswordByUsername(username);

            if (blogUser != null)
            {
                string storedPassword = blogUser.password;

                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(username, storedPassword, password);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    // Create and issue the authentication cookie
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false // Set to true for a persistent cookie
                    };

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, username),

                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = "Login Failed.";
                    return View("SignIn");

                }
            }

            ViewData["Error"] = "Login Failed.";
            return View("SignIn");

        }

        [AllowAnonymous]
        [HttpGet("/admin/signout")]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index"); // Redirect to the home page
        }

    }
}
