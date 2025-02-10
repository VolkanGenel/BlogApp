using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models
{
    public class CreatePostViewModel
    {
        public int PostId { get; set; }
        [Required]
        [Display(Name = "Post Title")]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public string? Url { get; set; }
        public bool IsActive { get; set; }
        public List<Tag> Tags { get; set; } = new();
    }
}