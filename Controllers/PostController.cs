using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;

        public PostController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            this._postRepository = postRepository;
            this._commentRepository = commentRepository;
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

        public async Task<IActionResult> Details(string url)
        {
            if (url == null)
            {
                return NotFound();
            }
            try
            {
                var entity = await _postRepository
                .Posts
                .Include(x => x.Tags)
                .Include(x=> x.Comments)
                .ThenInclude(x=> x.User)
                .FirstOrDefaultAsync(x => x.Url == url);

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
        [HttpPost]
        public JsonResult AddComment(int PostId, string UserName, string CommentText)
        {
            Comment comment = new Comment
            {
                CommentText = CommentText,
                PostId = PostId,
                PublishedAt = DateTime.UtcNow,
                User = new User {UserName = UserName, Image = "avatar.jpeg"}
            };
            _commentRepository.CreateComment(comment);
            // // return Redirect("/post/in-detail/"+ Url);
            // return RedirectToRoute("post_details", new {url = Url} );
            return Json (new {
                UserName,
                CommentText,
                comment.PublishedAt,
                comment.User.Image
            });
        }

    }
}
