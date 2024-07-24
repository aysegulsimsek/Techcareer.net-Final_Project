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

                    existingArticle.Title = article.Title;
                    existingArticle.Content = article.Content;

                    if (imageFile != null)
                    {
                        var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                        var extension = Path.GetExtension(imageFile.FileName)?.ToLowerInvariant();
                        if (extension == null || !allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("", "Geçerli bir resim seçiniz!");
                            return View(article);
                        }

                        // Eski resmin silinmesi
                        if (!string.IsNullOrEmpty(existingArticle.ImageFile))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", Path.GetFileName(existingArticle.ImageFile));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Yeni resmin kaydedilmesi
                        var randomFileName = $"{Guid.NewGuid()}{extension}";
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                        try
                        {
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(stream);
                            }
                            existingArticle.ImageFile = $"/img/{randomFileName}";
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", $"Dosya yüklenirken bir hata oluştu: {ex.Message}");
                            return View(article);
                        }
                    }

                    _context.Update(existingArticle);
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

        // GET: Delete
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: DeleteConfirmed
        [HttpPost, ActionName("DeleteConfirmed")]
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int articleId, string content)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }

            // İçeriği doğrula
            if (string.IsNullOrWhiteSpace(content))
            {
                ViewData["ErrorMessage"] = "Yorum içeriği boş olamaz!";
                return View("Details", article); // Yorum ekleme formunu da içeren detay sayfasını geri döndür
            }

            if (content.Length < 15 || content.Length > 100)
            {
                ViewData["ErrorMessage"] = "Yorum içeriği 15 ile 100 karakter arasında olmalıdır!";
                return View("Details", article); // Yorum ekleme formunu da içeren detay sayfasını geri döndür
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
        public async Task<IActionResult> Comments()
        {
            var comments = await _context.Comments
                                          .Include(c => c.Article)
                                          .ToListAsync();
            return View(comments);
        }

        // Edit Action (GET) for Comments
        // Edit Comment (GET) for Comments
        public async Task<IActionResult> EditComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // Edit Comment (POST) for Comments
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCommentPost(Comment comment)
        {
            // Yorumun bağlı olduğu makale var mı?
            if (comment.ArticleId <= 0 || !await _context.Articles.AnyAsync(a => a.Id == comment.ArticleId))
            {
                ViewData["ErrorMessage"] = "Geçersiz Makale ID'si";
                return View("EditComment", comment); // Hata mesajını geri döndür ve düzenleme formunu göster
            }

            // Yorum güncelleme işlemi
            var existingComment = await _context.Comments.FindAsync(comment.CommentId);
            if (existingComment == null)
            {
                return NotFound();
            }

            existingComment.Content = comment.Content;
            _context.Comments.Update(existingComment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = comment.ArticleId });
        }




        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }

        // GET: Delete Comment
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments
                                        .Include(c => c.Article)
                                        .FirstOrDefaultAsync(c => c.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Delete Comment
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCommentConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Comments));
        }


        // Create Action (GET)
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Article model, IFormFile? imageFile)
        {
            if (imageFile != null)
            {
                // Geçerli uzantıları belirleyin
                var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                var extension = Path.GetExtension(imageFile.FileName)?.ToLowerInvariant();

                // Uzantının geçerli olup olmadığını kontrol edin
                if (extension == null || !allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Geçerli bir resim seçiniz!");
                    return View(model); // Modeli geri döndürün, böylece kullanıcı form verilerini kaybetmez
                }

                // Yeni resim adı ve yolu belirleyin
                var randomFileName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                // Resim dosyasını kaydedin
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Modelin ImagePath özelliğini güncelleyin
                model.ImageFile = $"/img/{randomFileName}";
            }

            // Modeli veritabanına ekleyin ve kaydedin
            _context.Articles.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}