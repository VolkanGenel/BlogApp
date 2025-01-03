using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class NewPosts : ViewComponent
    {
        private IPostRepository _postRepository;
        public NewPosts(IPostRepository postRepository)
        {
            this._postRepository = postRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _postRepository.Posts.OrderByDescending(p=> p.PublishedAt).Take(5).ToListAsync());
        }
    }
}