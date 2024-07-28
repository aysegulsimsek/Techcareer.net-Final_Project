using DBlog.Entity;
using Microsoft.EntityFrameworkCore;

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
                            Name = "Ayşegül Şimşek",
                            Email = "info@asimsek.com",
                            Password = "123456",
                            Image = "p3.jpg"
                        },
                        new User
                        {
                            UserName = "userOne",
                            Name = "User One",
                            Email = "info@user.com",
                            Password = "123456",
                            Image = "p2.jpg"
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
                            ImageFile = "/img/2.jpg",
                            UserId = 3,
                            Comments = new List<Comment>
                            {
                                    new() { Content = "başarılı", UserId = 3, CommentDate = DateTime.Now },
                                    new() { Content = "başarılı, tavsiye ederim", UserId = 1, CommentDate = DateTime.Now }
                            }
                        },
                        new Article
                        {
                            Id = 2,
                            Title = "Second Article",
                            Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...",
                            PublishedDate = DateTime.Now.AddDays(-50),
                            ImageFile = "/img/3.jpg",
                            UserId = 1,
                            Comments = new List<Comment>
                            {
                                    new() { Content = "başarılı", UserId = 1, CommentDate = DateTime.Now },
                                    new() { Content = "başarılı, tavsiye ederim", UserId = 2, CommentDate = DateTime.Now }
                            }
                        },
                        new Article
                        {
                            Id = 3,
                            Title = "Third Article",
                            Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry...",
                            PublishedDate = DateTime.Now.AddDays(-80),
                            ImageFile = "/img/5G.jpg",
                            UserId = 2,
                            Comments = new List<Comment>
                            {
                                    new() { Content = "başarılı", UserId = 1, CommentDate = DateTime.Now },
                                    new() { Content = "başarılı, tavsiye ederim", UserId = 2, CommentDate = DateTime.Now }
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
                                    new() { Content = "başarılı", UserId = 1, CommentDate = DateTime.Now },
                                    new() { Content = "başarılı, tavsiye ederim", UserId = 2, CommentDate = DateTime.Now }
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
                                    new() { Content = "başarılı", UserId = 1, CommentDate = DateTime.Now },
                                    new() { Content = "başarılı, tavsiye ederim", UserId =2, CommentDate = DateTime.Now }
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
                                    new() { Content = "başarılı", UserId = 1, CommentDate = DateTime.Now },
                                    new() { Content = "başarılı, tavsiye ederim", UserId = 2, CommentDate = DateTime.Now }
                            }
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}

