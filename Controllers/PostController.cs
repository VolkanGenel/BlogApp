using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private IPostRepository _repository;
        public PostController (IPostRepository repository)
        {
            this._repository = repository;
        }
        // GET: PostController
        public IActionResult Index()
        {
            return View(_repository.Posts.ToList());
        }

    }
}
