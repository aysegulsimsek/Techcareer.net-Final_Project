using DBlog.Data;
using DBlog.Data.Abstract;
using DBlog.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

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

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

    }
}
