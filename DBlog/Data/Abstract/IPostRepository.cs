using DBlog.Entity;

namespace DBlog.Data.Abstract
{

    public interface IPostRepository
    {

        IQueryable<Article> Articles { get; }
        void CreatePost(Article article);
    }
}