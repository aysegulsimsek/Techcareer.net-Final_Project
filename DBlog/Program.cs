using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using DBlog.Data;
using DBlog.Models;
using DBlog.Data.Abstract;
using DBlog.Data.Concrete;
using DBlog.Data.Concrete.EfCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("database")));

builder.Services.AddScoped<IPostRepository, EfPostRepository>();
// builder.Services.AddScoped<ITagRepository, EfTagRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Users/Login";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

SeedData.TestVerileriniDoldur(app);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "article_details",
    pattern: "article/details/{articleId}",
    defaults: new { controller = "Article", action = "Details" });
app.MapControllerRoute(
    name: "editArticle",
    pattern: "article/edit/{id}",
    defaults: new { controller = "Article", action = "Edit" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Article}/{action=Home}/{id?}");
app.MapControllerRoute(
    name: "profile",
    pattern: "users/profile/{userId}",
    defaults: new { controller = "Users", action = "Profile" });



app.MapRazorPages();

app.Run();
