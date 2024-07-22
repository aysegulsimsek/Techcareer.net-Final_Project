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
                new Article { Id = 1, Title = "First Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now, ImageUrl = "https://image.shutterstock.com/image-vector/design-seamless-advertising-pattern-creative-600w-112354688.jpg" },
                new Article { Id = 2, Title = "Second Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now, ImageUrl = "https://fatstacksblog.com/wp-content/uploads/2019/11/Person-writing-article-nov26.jpg" },
                new Article { Id = 3, Title = "Third Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now, ImageUrl = "https://leverageedublog.s3.ap-south-1.amazonaws.com/blog/wp-content/uploads/2020/01/24145013/article-writing.jpg" },
                new Article { Id = 4, Title = "Fourth Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now, ImageUrl = "https://www.staceyeburke.com/wp-content/uploads/2018/06/publications1.jpg" },
                new Article { Id = 5, Title = "Fifth Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now, ImageUrl = "https://image.shutterstock.com/image-vector/design-seamless-advertising-pattern-creative-600w-112354688.jpg" },
                new Article { Id = 6, Title = "Sixth Article", Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...", PublishedDate = DateTime.Now, ImageUrl = "https://fatstacksblog.com/wp-content/uploads/2019/11/Person-writing-article-nov26.jpg" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
