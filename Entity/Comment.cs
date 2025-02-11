using System;

namespace BlogApp.Entity;

public class Comment
{
    public int CommentId { get; set; }
    public string? CommentText { get; set;}
    public DateTime PublishedAt { get; set;}
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
