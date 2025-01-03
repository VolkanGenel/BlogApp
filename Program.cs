using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BlogContext>(options=> {
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("postgre_connection");
    //options.UseSqlite(connectionString);
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IPostRepository, EfPostRepository>(); //Ben sanal olan IPostRepositoryi çağırdığımda bana gerçeği EfPostRepositoryi gönder. 
builder.Services.AddScoped<ITagRepository, EfTagRepository>(); //Ben sanal olan ITagRepositoryi çağırdığımda bana gerçeği EfTagRepositoryi gönder. 

var app = builder.Build();

app.UseStaticFiles(); //wwwroot altındaki dosyaları program için erişime açmış olduk.
// dotnet tool install microsoft.web.librarymanager.cli -g -v 2.1.175 (versiyonu ile indirdik.)
// dotnet tool list -g orada microsoft.web.librarymanager.cli (libmani global alan indirdik, buradan görebiliriz.)
// libman init -p cdnjs projeye libman.json dosyasını ekledik ki bizim için yönetsin
// libman install bootstrap@5.3.3 -d wwwroot/lib/bootstrap bootstrap dosyalarını wwwroota indirmiş olduk.

SeedData.InstallTestData(app);

app.MapDefaultControllerRoute();

// localhost://post/react
// localhost://post/php

app.MapControllerRoute(
    name: "post_details",
    pattern: "post/{url}",
    defaults: new {controller = "Post", action="Details" }
);

app.MapControllerRoute(
    name: "posts_by_tag",
    pattern: "post/tag/{tag}",
    defaults: new {controller = "Post", action="Index" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Post}/{action=Index}/{id?}"
);

app.Run();
