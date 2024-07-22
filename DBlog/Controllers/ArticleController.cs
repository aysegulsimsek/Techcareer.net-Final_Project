using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBlog.Models;
using DBlog.Data;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System;

namespace DBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index Action
        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles
                                         .Include(a => a.Comments)
                                         .ToListAsync();
            return View(articles);
        }

        // Details Action
        public async Task<IActionResult> Details(int id)
        {
            var article = await _context.Articles
                                  .Include(a => a.Comments)
                                  .FirstOrDefaultAsync(a => a.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // Create Action (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Create Action (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                    var extension = Path.GetExtension(imageFile.FileName)?.ToLowerInvariant(); // Ensure extension is not null
                    if (extension == null || !allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("", "Geçerli bir resim seçiniz!");
                    }
                    else
                    {
                        var randomFileName = $"{Guid.NewGuid()}{extension}";
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                        try
                        {
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(stream);
                            }
                            article.ImageUrl = $"/img/{randomFileName}";
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", $"Dosya yüklenirken bir hata oluştu: {ex.Message}");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bir dosya seçiniz.");
                }

                if (ModelState.IsValid)
                {
                    _context.Add(article);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(article);
        }

        // Edit Action (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // Edit Action (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article, IFormFile? imageFile)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingArticle = await _context.Articles.FindAsync(id);
                    if (existingArticle == null)
                    {
                        return NotFound();
                    }

                    if (imageFile != null)
                    {
                        var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                        var extension = Path.GetExtension(imageFile.FileName)?.ToLowerInvariant(); // Ensure extension is not null
                        if (extension == null || !allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("", "Geçerli bir resim seçiniz!");
                        }
                        else
                        {
                            var randomFileName = $"{Guid.NewGuid()}{extension}";
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                            try
                            {
                                using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    await imageFile.CopyToAsync(stream);
                                }
                                existingArticle.ImageUrl = $"/img/{randomFileName}";
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("", $"Dosya yüklenirken bir hata oluştu: {ex.Message}");
                            }
                        }
                    }

                    existingArticle.Title = article.Title;
                    existingArticle.Content = article.Content;

                    _context.Entry(existingArticle).CurrentValues.SetValues(article);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(article);
        }

        // Delete Action (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Comments)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // Delete Action (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Article deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        // AddComment Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int articleId, string content)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                Content = content,
                CommentDate = DateTime.Now,
                ArticleId = articleId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = articleId });
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
