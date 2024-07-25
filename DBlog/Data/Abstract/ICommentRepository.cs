using DBlog.Entity;

namespace DBlog.Data.Abstract
{

    public interface ICommentRepository
    {

        IQueryable<Comment> Comments { get; }
        void CreateComment(Comment comment);
    }
}