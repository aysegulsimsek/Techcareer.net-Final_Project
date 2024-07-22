using Microsoft.EntityFrameworkCore;
using DBlog.Models;

namespace DBlog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Article)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.ArticleId);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.User)
                .WithMany() // Kullanıcının makale koleksiyonuna sahip olmasını sağlıyoruz.
                .HasForeignKey(a => a.UserId);

            // Makale ve kullanıcı verilerini tanımlıyoruz.
            modelBuilder.Entity<Article>().HasData(
                new Article { Id = 1, Title = "First Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now, UserId = 1, ImageUrl = "https://image.shutterstock.com/image-vector/design-seamless-advertising-pattern-creative-600w-112354688.jpg" },
                new Article { Id = 2, Title = "Second Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now, UserId = 1, ImageUrl = "https://fatstacksblog.com/wp-content/uploads/2019/11/Person-writing-article-nov26.jpg" },
                new Article { Id = 3, Title = "Third Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now, UserId = 2, ImageUrl = "https://leverageedublog.s3.ap-south-1.amazonaws.com/blog/wp-content/uploads/2020/01/24145013/article-writing.jpg" },
                new Article { Id = 4, Title = "Fourth Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now, UserId = 2, ImageUrl = "https://th.bing.com/th/id/OIP.WwajPSJu5YunOsdTaD0hqwHaE7?rs=1&pid=ImgDetMain" },
                new Article { Id = 5, Title = "Fifth Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now, UserId = 3, ImageUrl = "https://image.shutterstock.com/image-vector/design-seamless-advertising-pattern-creative-600w-112354688.jpg" },
                new Article { Id = 6, Title = "Sixth Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now, UserId = 3, ImageUrl = "https://fatstacksblog.com/wp-content/uploads/2019/11/Person-writing-article-nov26.jpg" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, UserName = "User1", Email = "user1@example.com" },
                new User { UserId = 2, UserName = "User2", Email = "user2@example.com" },
                new User { UserId = 3, UserName = "User3", Email = "user3@example.com" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
