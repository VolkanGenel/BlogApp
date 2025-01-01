using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private IPostRepository _postRepository;

        public PostController (IPostRepository postRepository)
        {
            this._postRepository = postRepository;
        }
        // GET: PostController
        public IActionResult Index()
        {
            return View(
                new PostsViewModel 
                {
                    Posts = _postRepository.Posts.ToList()
                }    
            );
        }

    }
}
