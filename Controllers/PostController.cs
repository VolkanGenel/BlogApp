using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ITagRepository _tagRepository;


        public PostController(IPostRepository postRepository, ICommentRepository commentRepository, ITagRepository tagRepository)
        {
            this._postRepository = postRepository;
            this._commentRepository = commentRepository;
            this._tagRepository = tagRepository;
        }
        // GET: PostController
        public async Task<IActionResult> Index(string tag)
        {
            var posts = _postRepository.Posts.Where(x => x.IsActive == true);

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
                .Include(x => x.User)
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
        public JsonResult AddComment(int PostId, string CommentText)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

            Comment comment = new Comment
            {
                PostId = PostId,
                CommentText = CommentText,
                PublishedAt = DateTime.UtcNow,
                UserId = int.Parse(userId ?? "")
            };
            _commentRepository.CreateComment(comment);
            // // return Redirect("/post/in-detail/"+ Url);
            // return RedirectToRoute("post_details", new {url = Url} );
            return Json (new {
                username,
                CommentText,
                comment.PublishedAt,
                avatar
            });
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();   
        }

        [HttpPost]
        public IActionResult Create(CreatePostViewModel model)
        {
            
            if(ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _postRepository.CreatePost(
                    new Post {
                        Title = model.Title,
                        Content = model.Content,
                        Url = model.Url,
                        UserId = int.Parse(userId ?? ""),
                        PublishedAt = DateTime.UtcNow,
                        Image = "1.png",
                        IsActive = false
                    }
                );
                return RedirectToAction("Index");
            }
            return View(model); 
        }
        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            var posts = _postRepository.Posts;

            if(string.IsNullOrEmpty(role))
            {
                posts = posts.Where(x => x.UserId == userId);
            }

            return View(await posts.ToListAsync());
        }
        [Authorize]
        public IActionResult Edit (int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var post = _postRepository.Posts.Include(x => x.Tags).FirstOrDefault(x => x.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            ViewBag.Tags = _tagRepository.Tags.ToList();

            return View(new CreatePostViewModel {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive,
                Tags = post.Tags
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit (CreatePostViewModel model, int[] tagIds)
        {
            if(ModelState.IsValid)
            {
                var entityToUpdate = new Post {
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url
                };
                
                if(User.FindFirstValue(ClaimTypes.Role)=="admin")
                {
                    entityToUpdate.IsActive = model.IsActive;
                }


                _postRepository.EditPost(entityToUpdate, tagIds);
                return RedirectToAction("list");
            }
            ViewBag.Tags = _tagRepository.Tags.ToList();
            return View(model);
        }

    }
}
