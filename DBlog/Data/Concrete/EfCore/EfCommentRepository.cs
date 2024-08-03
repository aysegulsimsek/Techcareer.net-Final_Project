using DBlog.Data.Abstract;
using DBlog.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DBlog.Data.Concrete
{
    public class EfCommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public EfCommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Comment> Comments => _context.Comments.AsQueryable();


        public void EditComment(Comment comment)
        {
            var entity = _context.Comments.Include(i => i.Article).Include(c => c.User).FirstOrDefault(i => i.CommentId == comment.CommentId);
            if (entity != null)
            {
                entity.Content = comment.Content;
                _context.SaveChanges();
            }
        }
        public void Update(Comment comment)
        {
            var entity = _context.Comments.Include(a => a.Article).Include(c => c.User).FirstOrDefault(i => i.CommentId == comment.CommentId);
            if (entity != null)
            {
                entity.Content = comment.Content;
                _context.Comments.Update(entity);
                _context.SaveChanges();
            }
        }

        public void DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
        }
        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Comment>> GetUserCommentsAsync(int userId)
        {
            return await _context.Comments
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
        public void CreateComment(Comment entity)
        {
            try
            {
                _context.Comments.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Comment?> FindAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Article)
                .FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Remove(Comment comment)
        {
            _context.Comments.Remove(comment);
        }
    }
}
