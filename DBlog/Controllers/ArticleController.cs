using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DBlog.Models;
using Microsoft.AspNetCore.Authorization;
using DBlog.Data.Abstract;
using DBlog.Data;
using DBlog.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using DBlog.ViewModels;
using System.Text.Json;
namespace DBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public ArticleController(ICommentRepository commentRepository, IPostRepository postRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        // Home
        public IActionResult Home()
        {
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


        // Index
        public async Task<IActionResult> Index()
        {
            var articles = await _postRepository.Articles
                .Include(a => a.Comments)
                .ToListAsync();

            return View(articles);
        }

        public async Task<IActionResult> Details(int articleId)
        {


            var article = await _postRepository.Articles
                .Include(x => x.User)
                .Include(x => x.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Id == articleId);

            if (article == null)
            {
                return NotFound("Article not found.");
            }

            return View(article);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid article ID.");
            }

            var article = await _postRepository.Articles
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return NotFound("Article not found.");
            }

            return View(article);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Article model)
        {
            if (model == null)
            {
                return BadRequest("Invalid model data.");
            }


            var article = await _postRepository.Articles
                .FirstOrDefaultAsync(a => a.Id == model.Id);

            if (article == null)
            {
                return NotFound("Article not found.");
            }


            article.Title = model.Title;
            article.Content = model.Content;

            await _postRepository.SaveChangesAsync();

            return RedirectToAction("Details", new { id = article.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var article = await _postRepository.Articles
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return NotFound("Article not found.");
            }

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _postRepository.Articles
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return NotFound("Article not found.");
            }

            _postRepository.Articles.Remove(article);
            await _postRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var article = new Article
            {
                Title = model.Title,
                Content = model.Content,
                PublishedDate = DateTime.Now,
                UserId = int.Parse(userId)
            };

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(model.ImageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                article.ImageFile = $"/img/{fileName}";
            }

            _postRepository.Articles.Add(article);
            await _postRepository.SaveChangesAsync();

            return RedirectToAction("Index", "Article");
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
        public async Task<IActionResult> AddComment(AddCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");

            var comment = new Comment
            {
                Content = model.Content,
                UserId = userId,
                ArticleId = model.ArticleId,
                CommentDate = DateTime.UtcNow
            };

            await _commentRepository.AddCommentAsync(comment);

            return RedirectToAction("Details", "Article", new { id = model.ArticleId });
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

            return View(comment);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(int id, [Bind("CommentId,Content")] Comment comment, User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingComment = await _commentRepository.Comments
                        .Include(c => c.User)
                        .Include(c => c.Article)
                        .FirstOrDefaultAsync(c => c.CommentId == id);

                    if (existingComment == null)
                    {
                        return NotFound();
                    }

                    comment.User = existingComment.User;
                    comment.Article = existingComment.Article;
                    comment.UserId = existingComment.UserId;
                    comment.ArticleId = existingComment.ArticleId;

                    existingComment.Content = comment.Content;

                    _commentRepository.Update(existingComment);
                    await _commentRepository.SaveChangesAsync();
                    return RedirectToAction(nameof(GetComments));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!_commentRepository.Comments.Any(e => e.CommentId == comment.CommentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        Console.WriteLine($"Concurrency exception: {ex.Message}");
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
            else
            {
                ModelState.Remove("User");
                ModelState.Remove("Article");

                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState error: {error.ErrorMessage}");
                }

                return View(comment);
            }
        }





        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepository.Comments
                 .FirstOrDefaultAsync(c => c.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        [HttpPost, ActionName("DeleteComment")]
        public async Task<IActionResult> DeleteCommentConfirmed(int id)
        {
            var comment = await _commentRepository.FindAsync(id);
            if (comment != null)
            {
                _commentRepository.DeleteComment(comment);
                await _commentRepository.SaveChangesAsync();
            }
            return RedirectToAction("ListComments");
        }


    }

}


