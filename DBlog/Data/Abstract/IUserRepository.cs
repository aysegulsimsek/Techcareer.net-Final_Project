using DBlog.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DBlog.Data.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        void CreateUser(User user);
        Task<User?> GetUserById(int id);

        Task SaveChangesAsync();
        Task UpdateUser(User user);
    }
}
