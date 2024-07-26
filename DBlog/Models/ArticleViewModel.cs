using DBlog.Entity;

namespace DBlog.ViewModels
{
    public class ArticleViewModel
    {
        public List<Article> Articles { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }
}
