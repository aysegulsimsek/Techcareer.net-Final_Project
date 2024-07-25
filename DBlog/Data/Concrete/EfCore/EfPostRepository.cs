using DBlog.Data.Abstract;
using DBlog.Entity;

namespace DBlog.Data.Concrete
{

    public class EfPostRepository : IPostRepository
    {
        private ApplicationDbContext _context;

        public EfPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Article> Articles => _context.Articles;

        public void CreatePost(Article article)
        {
            _context.Articles.Add(article);
            _context.SaveChanges();
        }
    }
}