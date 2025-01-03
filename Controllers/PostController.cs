using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            this._postRepository = postRepository;
        }
        // GET: PostController
        public async Task<IActionResult> Index(string tag)
        {
            var posts = _postRepository.Posts;

            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(x => x.Tags.Any(x => x.Url == tag));
            }
                return View(
                    new PostsViewModel
                    {
                        Posts = await posts.ToListAsync()
                    }
                );

        }

        public async Task<IActionResult> Details(string? url)
        {
            if (url == null)
            {
                return NotFound();
            }
            try
            {
                var entity = await _postRepository.Posts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Url == url);
                if (entity == null)
                {
                    return NotFound();
                }
                return View(entity);
            }
            catch (System.Exception)
            {

                return NotFound();
            }
        }

    }
}
