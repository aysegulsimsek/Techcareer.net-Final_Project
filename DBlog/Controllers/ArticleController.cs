using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DBlog.Data.Abstract;
using DBlog.Data;
using DBlog.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using DBlog.Models;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace DBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private ITagRepository _tagRepository;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(
            ILogger<ArticleController> logger,
            ICommentRepository commentRepository,
            IPostRepository postRepository,
            IUserRepository userRepository,
            ITagRepository tagRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _logger = logger;
        }

        public IActionResult Home()
        {
            var claims = User.Claims;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MyBlog()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var articles = await _postRepository.Articles
                .Where(a => a.UserId == userId)
                .ToListAsync();
            var articleViewModel = new ArticleViewModel
            {
                Articles = articles,
                ErrorMessage = null
            };

            return View(articleViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Index(string tag)
        {
            IQueryable<Article> posts = _postRepository.Articles
                .Include(a => a.Tags)
                .Include(a => a.Comments)
                .Where(i => i.IsActive);

            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.Url == tag));
            }

            var viewModel = new ArticleViewModel
            {
                Articles = await posts.ToListAsync(),
                Tags = await _tagRepository.Tags.ToListAsync()
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Details(string url)
        {
            return View(await _postRepository.Articles
                .Include(x => x.User)
                .Include(x => x.Tags)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .FirstOrDefaultAsync(p => p.Url == url));
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = _postRepository.Articles.Include(x => x.Tags).FirstOrDefault(i => i.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            ViewBag.Tags = _tagRepository.Tags.ToList();
            return View(new ArticleCreateViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Url = article.Url,
                IsActive = article.IsActive,
                ExistingImageFile = article.ImageFile
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(ArticleCreateViewModel model, int[] tagIds)
        {
            if (ModelState.IsValid)
            {
                var existingArticle = await _postRepository.FindAsync(model.Id);
                if (existingArticle == null)
                {
                    return NotFound();
                }

                existingArticle.Title = model.Title;
                existingArticle.Content = model.Content;
                existingArticle.Url = model.Url;

                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    existingArticle.IsActive = model.IsActive;
                }

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingArticle.ImageFile))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", existingArticle.ImageFile);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    existingArticle.ImageFile = uniqueFileName;
                }

                var currentTagIds = existingArticle.Tags.Select(t => t.TagId).ToList();
                var newTags = _tagRepository.Tags.Where(tag => tagIds.Contains(tag.TagId) && !currentTagIds.Contains(tag.TagId)).ToList();

                foreach (var tag in newTags)
                {
                    existingArticle.Tags.Add(tag);
                }

                _postRepository.Update(existingArticle);

                return RedirectToAction("Index");
            }

            ViewBag.Tags = _tagRepository.Tags.ToList();
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _postRepository.Articles.FirstOrDefaultAsync(a => a.Id == id);
            if (article == null)
            {
                return NotFound("Article not found.");
            }

            return View(article);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _postRepository.Articles.FirstOrDefaultAsync(a => a.Id == id);
            if (article == null)
            {
                return NotFound("Article not found.");
            }

            _postRepository.Articles.Remove(article);
            await _postRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? fileName = null;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ImageFile", "Lütfen yalnızca resim dosyası yükleyin.");
                        return View(model);
                    }

                    fileName = model.ImageFile.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                }

                var article = new Article
                {
                    Title = model.Title,
                    Content = model.Content,
                    Url = model.Url,
                    IsActive = false,
                    PublishedDate = DateTime.Now,
                    ImageFile = fileName != null ? $"/img/{fileName}" : null,
                    UserId = int.Parse(userId ?? "")
                };

                _postRepository.Articles.Add(article);
                await _postRepository.SaveChangesAsync();

                return RedirectToAction("Index", "Article");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _commentRepository.Comments
                .Include(c => c.Article)
                .Include(c => c.User)
                .ToListAsync();

            return View(comments);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddComment(int ArticleId, string Content)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

            var entity = new Comment
            {
                ArticleId = ArticleId,
                Content = Content,
                CommentDate = DateTime.Now,
                UserId = int.Parse(userId ?? "")
            };

            _commentRepository.CreateComment(entity);

            return Json(new
            {
                username,
                Content,
                entity.CommentDate,
                avatar
            });
        }

        [Authorize]
        public async Task<IActionResult> EditComment(int id)
        {
            var comment = await _commentRepository.Comments
                .Include(c => c.User)
                .Include(c => c.Article)
                .FirstOrDefaultAsync(c => c.CommentId == id);

            if (comment == null)
            {
                return NotFound();
            }

            if (comment.User == null || comment.Article == null)
            {
                ModelState.AddModelError("", "İlgili kullanıcı veya makale bulunamadı.");
                return View(comment);
            }

            return View(comment);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditComment(int id, Comment model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingComment = await _commentRepository.Comments
                .Include(c => c.User)
                .Include(c => c.Article)
                .FirstOrDefaultAsync(c => c.CommentId == id);

            if (existingComment == null)
            {
                return NotFound();
            }

            existingComment.Content = model.Content;

            try
            {
                _commentRepository.Update(existingComment);
                await _commentRepository.SaveChangesAsync();

                TempData["SuccessMessage"] = "Yorum başarıyla güncellendi!";
                return RedirectToAction("GetComments");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Yorum güncellenirken bir hata oluştu: " + ex.Message);
                return View(model);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepository.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedComment(int id)
        {
            var comment = await _commentRepository.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            try
            {
                _commentRepository.Remove(comment);
                await _commentRepository.SaveChangesAsync();

                TempData["SuccessMessage"] = "Comment deleted successfully!";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the comment: " + ex.Message);
                return View(comment);
            }

            if (User.IsInRole("admin"))
            {
                return RedirectToAction("GetComments");
            }
            else
            {
                return RedirectToAction("Profile", "Users", new { username = User.Identity.Name });
            }
        }
    }
}
