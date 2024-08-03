using DBlog.Entity;

namespace DBlog.Models
{
    public class ArticleViewModel
    {
        public List<Article> Articles { get; set; } = new();
        public IFormFile? ImageFile { get; set; }


        public List<Comment> Comments { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
        public List<User> Users { get; set; } = new();

        public string? ErrorMessage { get; set; }
    }
}


