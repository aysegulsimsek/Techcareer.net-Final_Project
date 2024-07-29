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

        public void CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
        }
        public void Update(Comment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
        }
        public void EditComment(Comment comment)
        {
            _context.Comments.Update(comment);
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
        public async Task<Comment?> FindAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Comment>> GetUserCommentsAsync(int userId)
        {
            return await _context.Comments
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
    }
}
