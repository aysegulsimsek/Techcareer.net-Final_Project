using System.Linq;
using System.Threading.Tasks;
using DBlog.Data.Abstract;
using DBlog.Entity;
using Microsoft.EntityFrameworkCore;

namespace DBlog.Data.Concrete
{
    public class EfPostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public EfPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public DbSet<Article> Articles => _context.Articles;

        public void CreatePost(Article article)
        {
            _context.Articles.Add(article);
            _context.SaveChanges();
        }

        public void EditPost(Article article)
        {
            _context.Articles.Update(article);
        }

        public void Update(Article article)
        {
            _context.Articles.Update(article);
        }

        public async Task<Article?> FindAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
