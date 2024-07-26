using DBlog.Data;
using DBlog.Data.Abstract;
using DBlog.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DBlog.Data.Concrete
{
    public class EfUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public EfUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<User> Users => _context.Users;

        // public async Task<User?> GetUserById(int id)
        // {
        //     var user = await _context.Users.FindAsync(id);
        //     return user; // Kullanıcı bulunamazsa null döndür
        // }
        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task UpdateUser(User user)
        {
            // Assuming you need to update user details
            _context.Users.Update(user);
            return SaveChangesAsync();
        }
    }
}
