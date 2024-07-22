using Microsoft.EntityFrameworkCore;
using DBlog.Data;
using DBlog.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Razor Pages hizmetlerini ekleyin
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("database")));

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    await SeedData(context); // SeedData fonksiyonunu çağırın
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Razor Pages rotalarını haritalandırın

app.Run();

static async Task SeedData(ApplicationDbContext context)
{
    // User ekleme
    var user = new User
    {
        UserName = "admin",
        Email = "admin@example.com",
        Password = "password" // Düz metin şifreleme önerilmez, bu sadece örnek amaçlıdır.
    };

    if (!await context.Users.AnyAsync(u => u.Email == user.Email))
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }
    else
    {
        user = await context.Users.FirstAsync(u => u.Email == user.Email);
    }

    // Article ekleme
    if (!await context.Articles.AnyAsync())
    {
        context.Articles.AddRange(
            new Article
            {
                Title = "First Article",
                Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                PublishedDate = DateTime.Now,
                ImageUrl = "https://th.bing.com/th/id/OIP.d36dPbz5o2alFsmVos4hkwHaEK?rs=1&pid=ImgDetMain",
                UserId = user.UserId // Kullanıcı ID'si ile ilişkilendirme
            },
            new Article
            {
                Title = "Second Article",
                Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                PublishedDate = DateTime.Now,
                ImageUrl = "https://image.shutterstock.com/image-vector/design-seamless-advertising-pattern-creative-600w-112354688.jpg",
                UserId = user.UserId // Kullanıcı ID'si ile ilişkilendirme
            }
        );
        await context.SaveChangesAsync();
    }
}
