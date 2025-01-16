using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void InstallTestData(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Entity.Tag { Text = "web programming", Url = "web-programming", Color = TagColors.warning },
                        new Entity.Tag { Text = "backend", Url = "backend", Color = TagColors.info },
                        new Entity.Tag { Text = "frontend", Url = "frontend", Color = TagColors.success },
                        new Entity.Tag { Text = "fullstack", Url = "fullstack", Color = TagColors.secondary },
                        new Entity.Tag { Text = "php", Url = "php", Color = TagColors.primary }
                    );
                    context.SaveChanges();
                }
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new Entity.User { UserName = "sadikturan", Name="Sadık Turan", Email="info@sadikturan.com", Password= "123456", Image="p1.jpeg" },
                        new Entity.User { UserName = "muhammetkaya", Name="Muhammet Kaya", Email="info@muhammetkaya.com", Password= "123456", Image="p2.jpeg" },
                        new Entity.User { UserName = "volkangenel", Name="Volkan Genel", Email="info@volkangenel.com", Password= "123456", Image="p3.jpeg" }
                    );
                    context.SaveChanges();
                }
                if (!context.Posts.Any())
                {
                    context.AddRange(
                        new Post
                        {
                            Title = "Asp.net core",

                            Content = "Asp.new core lectures",
                            Description = "Asp.new core lectures",
                            Url = "aspnet-core",
                            IsActive = true,
                            PublishedAt = DateTime.UtcNow.AddDays(-10),
                            Tags = context.Tags.OrderBy(t => t.TagId).Take(3).ToList(),
                            Image = "1.png",
                            UserId = 1,
                            Comments = new List<Comment> { 
                                new Comment {CommentText = "Nice Course", PublishedAt= DateTime.UtcNow.AddHours(-10), UserId = 1},
                                new Comment {CommentText = "Like it so much", PublishedAt= DateTime.UtcNow, UserId = 2}
                            }
                        },
                        new Post
                        {
                            Title = "Php",
                            Content = "Php lectures",
                            Description = "Php lectures",
                            Url = "php",
                            IsActive = true,
                            PublishedAt = DateTime.UtcNow.AddDays(-20),
                            Tags = context.Tags.OrderBy(t => t.TagId).Take(2).ToList(),
                            Image = "2.png",
                            UserId = 2
                        },
                        new Post
                        {
                            Title = "Django",
                            Content = "Django lectures",
                            Description = "Django lectures",
                            Url = "django",
                            IsActive = true,
                            PublishedAt = DateTime.UtcNow.AddDays(-5),
                            Tags = context.Tags.OrderBy(t => t.TagId).Take(4).ToList(),
                            Image = "3.png",
                            UserId = 3
                        },
                        new Post
                        {
                            Title = "React",
                            Content = "React lectures",
                            Description = "React lectures",
                            Url = "react",
                            IsActive = true,
                            PublishedAt = DateTime.UtcNow.AddDays(-4),
                            Tags = context.Tags.OrderBy(t => t.TagId).Take(4).ToList(),
                            Image = "3.png",
                            UserId = 3
                        },
                        new Post
                        {
                            Title = "Angular",
                            Content = "Angular lectures",
                            Description = "Angular lectures",
                            Url = "angular",
                            IsActive = true,
                            PublishedAt = DateTime.UtcNow.AddDays(-12),
                            Tags = context.Tags.OrderBy(t => t.TagId).Take(4).ToList(),
                            Image = "3.png",
                            UserId = 3
                        },
                        new Post
                        {
                            Title = "Java",
                            Content = "Java lectures",
                            Description = "Java lectures",
                            Url = "java",
                            IsActive = true,
                            PublishedAt = DateTime.UtcNow.AddDays(-30),
                            Tags = context.Tags.OrderBy(t => t.TagId).Take(4).ToList(),
                            Image = "3.png",
                            UserId = 3
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}