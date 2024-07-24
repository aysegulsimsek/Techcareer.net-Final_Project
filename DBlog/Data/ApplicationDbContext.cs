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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Article)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.ArticleId);

            modelBuilder.Entity<Article>().HasData(
                new Article { Id = 1, Title = "First Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now.AddDays(-10), ImageFile = "/img/1.jpg" },
                new Article { Id = 2, Title = "Second Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now.AddDays(-50), ImageFile = "/img/6.jpg" },
                new Article { Id = 3, Title = "Third Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now.AddDays(-80), ImageFile = "/img/4.jpg" },
                new Article { Id = 4, Title = "Fourth Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now.AddDays(-50), ImageFile = "/img/7.jpg" },
                new Article { Id = 5, Title = "Fifth Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now.AddDays(-30), ImageFile = "/img/5.jpg" },
                new Article { Id = 6, Title = "Sixth Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now.AddDays(-20), ImageFile = "/img/6.jpg" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}