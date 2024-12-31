using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
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

var app = builder.Build();

SeedData.InstallTestData(app);

app.MapDefaultControllerRoute();

app.Run();
