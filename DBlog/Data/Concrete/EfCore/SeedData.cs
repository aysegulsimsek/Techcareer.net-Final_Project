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
                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "ai-future", Url = "ai-future", Color = TagColors.primary },
                        new Tag { Text = "Blockchain-Kripto", Url = "Blockchain-Kripto", Color = TagColors.danger },
                        new Tag { Text = "Cyber-Security", Url = "Cyber-Security", Color = TagColors.info },
                        new Tag { Text = "5G Teknolojisi", Url = "5G Teknolojisi", Color = TagColors.success },
                        new Tag { Text = "Quantum Computing", Url = "Quantum Computing", Color = TagColors.secondary }
                    );
                    context.SaveChanges();
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
                         Title = "Yapay Zeka: Geleceğin Teknolojisi",
                         Content = "Yapay zeka, geleceğin teknolojisi olarak birçok alanda devrim niteliğinde değişikliklere yol açmaktadır. Sağlık hizmetlerinden finansa, eğitimden ulaşıma kadar birçok sektörde AI'nın etkilerini görmekteyiz. Ancak, bu teknolojinin etik ve toplumsal boyutlarının da göz ardı edilmemesi gerekmektedir. Gelecekte yapay zeka, yaşamımızın her alanında daha da yaygınlaşacak ve hayatımızı köklü bir şekilde değiştirecektir.",
                         Url = "ai-future",
                         IsActive = true,
                         PublishedDate = DateTime.Now.AddDays(-15),
                         Tags = context.Tags.Take(3).ToList(),
                         ImageFile = "ai.jpg",
                         UserId = 1,
                         Comments = new List<Comment>{
                                                        new Comment {Content = "başarılı", CommentDate = new DateTime(),UserId=1},
                                                        new Comment {Content = "başarılı, tavsiye ederim", CommentDate = new DateTime(),UserId=2}
                                                    }
                     },
                        new Article
                        {
                            Title = "Blockchain ve Kripto Paraların Dünyası",
                            Content = "Kripto paraların geleceği, finans dünyasında büyük bir potansiyele sahiptir. Merkezi olmayan finans (DeFi) uygulamaları, finansal hizmetlere erişimi demokratikleştirmekte ve herkes için erişilebilir hale getirmektedir. Ayrıca, merkez bankalarının dijital para birimleri (CBDC) geliştirme çalışmaları, kripto paraların gelecekte daha da yaygınlaşacağını göstermektedir günümüzün en yenilikçi teknolojilerinden ikisidir.Bu teknolojiler, finans sektöründen sağlık hizmetlerine, tedarik zinciri yönetiminden dijital kimlik yönetimine kadar birçok alanda devrim yaratmaktadır.Gelecekte,blockchain ve kripto paraların daha da yaygınlaşması ve hayatımızın birçok alanında önemli bir rol oynaması beklenmektedir.Bu teknolojilerin getirdiği fırsatları ve beraberinde getirdiği zorlukları anlamak,geleceğe hazırlıklı olmanın anahtarıdır.",
                            Url = "Blockchain-Kripto",
                            IsActive = true,
                            PublishedDate = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(2).ToList(),
                            ImageFile = "3.jpg",
                            UserId = 2
                        },
                        new Article
                        {
                            Title = "Siber Güvenlik: Günümüzde ve Gelecekte Tehditler",
                            Content = "Siber güvenlik, günümüzde dijital dünyanın en önemli konularından biridir ve gelecekte de önemini koruyacaktır. Teknolojinin hızla gelişmesi, siber tehditlerin de aynı hızla evrim geçirmesine neden olmaktadır. Bu bağlamda, bireyler, kurumlar ve devletler, siber güvenliği sağlamak için proaktif önlemler almalı ve sürekli olarak kendilerini güncellemeleri gerekmektedir. Yapay zeka, makine öğrenimi ve blockchain gibi yeni teknolojiler, siber güvenlik savunmalarını güçlendirme potansiyeline sahipken, aynı zamanda yeni saldırı vektörleri de yaratabilir. Gelecekte, siber tehditlere karşı daha entegre, çok katmanlı ve ileri düzey savunma stratejileri geliştirmek, dijital dünyanın güvenliğini sağlamak için hayati önem taşıyacaktır. Siber güvenlik bilincinin artırılması, eğitimlerin yaygınlaştırılması ve uluslararası işbirliklerinin güçlendirilmesi, bu mücadelenin başarılı olmasında kritik rol oynayacaktır.",
                            Url = "Cyber-Security",
                            IsActive = true,
                            PublishedDate = DateTime.Now.AddDays(-5),
                            Tags = context.Tags.Take(2).ToList(),
                            ImageFile = "1.jpeg",
                            UserId = 1
                        }

                    );
                    context.SaveChanges();
                }
            }
        }
    }
}

