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
            var entity = _context.Articles.FirstOrDefault(i => i.Id == article.Id);
            if (entity != null)
            {
                entity.Title = article.Title;
                entity.Content = article.Content;
                entity.Url = article.Url;
                entity.IsActive = article.IsActive;

                _context.SaveChanges();
            }
        }

        public void EditPost(Article article, int[] tagIds)
        {
            var entity = _context.Articles.Include(i => i.Tags).FirstOrDefault(i => i.Id == article.Id);
            if (entity != null)
            {
                entity.Title = article.Title;
                entity.Content = article.Content;
                entity.Url = article.Url;
                entity.IsActive = article.IsActive;

                // Mevcut etiketlerle birleştir
                var currentTagIds = entity.Tags.Select(t => t.TagId).ToList();
                var newTags = _context.Tags.Where(tag => tagIds.Contains(tag.TagId) && !currentTagIds.Contains(tag.TagId)).ToList();

                foreach (var tag in newTags)
                {
                    entity.Tags.Add(tag);
                }

                _context.SaveChanges();
            }
        }

        public void Update(Article article)
        {
            var entity = _context.Articles.Include(a => a.Tags).FirstOrDefault(i => i.Id == article.Id);
            if (entity != null)
            {
                entity.Title = article.Title;
                entity.Content = article.Content;
                entity.Url = article.Url;
                entity.IsActive = article.IsActive;
                entity.ImageFile = article.ImageFile;

                // Mevcut etiketlerle birleştir
                var currentTagIds = entity.Tags.Select(t => t.TagId).ToList();
                var newTags = article.Tags.Where(tag => !currentTagIds.Contains(tag.TagId)).ToList();

                foreach (var tag in newTags)
                {
                    entity.Tags.Add(tag);
                }

                _context.Articles.Update(entity);
                _context.SaveChanges();
            }
        }

        public async Task<Article?> FindAsync(int? id)
        {
            if (id.HasValue)
            {
                return await _context.Articles.Include(a => a.Tags).FirstOrDefaultAsync(a => a.Id == id.Value);
            }
            return null;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
