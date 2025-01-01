using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class TagController : Controller
    {
        private ITagRepository _repository;
        public TagController (ITagRepository repository)
        {
            this._repository = repository;
        }
        // GET: TagController
        public IActionResult Index()
        {
            return View(_repository.Tags.ToList());
        }

    }
}
