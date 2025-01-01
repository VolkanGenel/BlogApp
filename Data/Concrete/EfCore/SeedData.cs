using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;
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
                        new Entity.Tag { Text = "web programming" },
                        new Entity.Tag { Text = "backend" },
                        new Entity.Tag { Text = "frontend" },
                        new Entity.Tag { Text = "fullstack" },
                        new Entity.Tag { Text = "php" }
                    );
                    context.SaveChanges();
                }
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new Entity.User { UserName = "sadikturan" },
                        new Entity.User { UserName = "muhammetkaya" },
                        new Entity.User { UserName = "volkangenel" }
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
                            IsActive = true,
                            PublishedAt = DateTime.UtcNow.AddDays(-10),
                            Tags = context.Tags.OrderBy(t => t.TagId).Take(3).ToList(),
                            Image = "1.png",
                            UserId = 1
                        },
                        new Post
                        {
                            Title = "Php",
                            Content = "Php lectures",
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
                            IsActive = true,
                            PublishedAt = DateTime.UtcNow.AddDays(-5),
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