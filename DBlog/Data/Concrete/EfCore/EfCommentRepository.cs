using DBlog.Data.Abstract;
using DBlog.Entity;

namespace DBlog.Data.Concrete
{

    public class EfCommentRepository : ICommentRepository
    {
        private ApplicationDbContext _context;

        public EfCommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Comment> Comments => _context.Comments;

        public void CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
    }
}