using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.ViewComponents
{
    public class TagsMenu: ViewComponent
    {
        private ITagRepository _tagrepository;

        public TagsMenu(ITagRepository tagRepository)
        {
            this._tagrepository = tagRepository;
        }
        public IViewComponentResult Invoke()
        {
            return View(_tagrepository.Tags.ToList());
        }
    }
}