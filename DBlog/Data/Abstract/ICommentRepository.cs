using System.Linq;
using System.Threading.Tasks;
using DBlog.Entity;

namespace DBlog.Data.Abstract
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }

        Task AddCommentAsync(Comment comment);
        void CreateComment(Comment comment);
        void EditComment(Comment comment);
        void DeleteComment(Comment comment);
        Task<Comment?> FindAsync(int id);
        Task<int> SaveChangesAsync();
        void Update(Comment comment);
    }
}
