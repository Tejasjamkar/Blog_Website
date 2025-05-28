using Bloggie.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task< IActionResult> Index(string urlHandle)
        {
          var blogPost =await  blogPostRepository.GetByUrlhandleAsync(urlHandle);
            return View(blogPost);
        }
    }
}
