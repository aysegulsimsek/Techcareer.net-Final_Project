using System.Threading.Tasks;
using DBlog.Entity;
using Microsoft.EntityFrameworkCore;

namespace DBlog.Data.Abstract
{
    public interface IPostRepository
    {
        DbSet<Article> Articles { get; }
        void CreatePost(Article article);
        void EditPost(Article article);
        void EditPost(Article article, int[] tagIds);
        Task<Article?> FindAsync(int? id);
        Task<int> SaveChangesAsync();
        void Update(Article article);
    }
}
