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

        // Articles özelliğini DbSet<Article> olarak implement edin
        public DbSet<Article> Articles => _context.Articles;

        public void CreatePost(Article article)
        {
            _context.Articles.Add(article);
            _context.SaveChanges(); // Consider using SaveChangesAsync for async consistency
        }

        public void EditPost(Article article)
        {
            _context.Articles.Update(article);
            // You may consider saving changes here or at a higher level
            // _context.SaveChanges();
        }

        public void Update(Article article)
        {
            _context.Articles.Update(article);
            // Again, consider saving changes here or at a higher level
            // _context.SaveChanges();
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
