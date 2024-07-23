using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBlog.Models;
using DBlog.Data;
using System.Threading.Tasks;
using System.Linq;

namespace DBlog.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Comment/Index
        public async Task<IActionResult> Index()
        {
            var comments = await _context.Comments
                                         .Include(c => c.Article)  // Yorumun ilişkili olduğu makaleyi de getir
                                         .OrderByDescending(c => c.CommentDate)  // Yorumları tarihine göre azalan sırada getir
                                         .ToListAsync();
            return View(comments);
        }
    }
}
