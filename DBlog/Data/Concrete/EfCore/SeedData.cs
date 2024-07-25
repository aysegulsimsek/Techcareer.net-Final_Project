using DBlog.Data;
using DBlog.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DBlog.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<ApplicationDbContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User
                        {
                            UserName = "admin",
                            Name = "Admin",
                            Email = "info@admin.com",
                            Image = "p2.jpg",
                            IsAdmin = true
                        },
                        new User
                        {
                            UserName = "userOne",
                            Name = "UserOne",
                            Email = "info@userone.com",
                            Image = "p1.jpg",
                            IsAdmin = false
                        }
                    );
                    context.SaveChanges();
                }

                if (!context.Articles.Any())
                {
                    context.Articles.AddRange(
                        new Article
                        {
                            Id = 1,
                            Title = "First Article",
                            Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...",
                            PublishedDate = DateTime.Now.AddDays(-10),
                            ImageFile = "/img/1.jpg",
                            UserId = 1,
                            Comments = new List<Comment>
                            {
                                new Comment { Content = "başarılı", UserId = 1 },
                                new Comment { Content = "başarılı, tavsiye ederim", UserId = 2 }
                            }
                        },
                        new Article
                        {
                            Id = 2,
                            Title = "Second Article",
                            Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...",
                            PublishedDate = DateTime.Now.AddDays(-50),
                            ImageFile = "/img/6.jpg",
                            UserId = 1,
                            Comments = new List<Comment>
                            {
                                new Comment { Content = "başarılı", UserId = 1 },
                                new Comment { Content = "başarılı, tavsiye ederim", UserId = 2 }
                            }
                        },
                        new Article
                        {
                            Id = 3,
                            Title = "Third Article",
                            Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...",
                            PublishedDate = DateTime.Now.AddDays(-80),
                            ImageFile = "/img/4.jpg",
                            UserId = 2,
                            Comments = new List<Comment>
                            {
                                new Comment { Content = "başarılı", UserId = 1 },
                                new Comment { Content = "başarılı, tavsiye ederim", UserId = 2 }
                            }
                        },
                        new Article
                        {
                            Id = 4,
                            Title = "Fourth Article",
                            Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...",
                            PublishedDate = DateTime.Now.AddDays(-50),
                            ImageFile = "/img/7.jpg",
                            UserId = 1,
                            Comments = new List<Comment>
                            {
                                new Comment { Content = "başarılı", UserId = 1 },
                                new Comment { Content = "başarılı, tavsiye ederim", UserId = 2 }
                            }
                        },
                        new Article
                        {
                            Id = 5,
                            Title = "Fifth Article",
                            Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...",
                            PublishedDate = DateTime.Now.AddDays(-30),
                            ImageFile = "/img/5.jpg",
                            UserId = 2,
                            Comments = new List<Comment>
                            {
                                new Comment { Content = "başarılı", UserId = 1 },
                                new Comment { Content = "başarılı, tavsiye ederim", UserId = 2 }
                            }
                        },
                        new Article
                        {
                            Id = 6,
                            Title = "Sixth Article",
                            Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...",
                            PublishedDate = DateTime.Now.AddDays(-20),
                            ImageFile = "/img/6.jpg",
                            UserId = 2,
                            Comments = new List<Comment>
                            {
                                new Comment { Content = "başarılı", UserId = 1 },
                                new Comment { Content = "başarılı, tavsiye ederim", UserId = 2 }
                            }
                        }
                    );
                    context.SaveChanges();
                }
            }
        }



    }
}
