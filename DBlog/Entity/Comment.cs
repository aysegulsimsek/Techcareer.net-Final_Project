using System.ComponentModel.DataAnnotations;

namespace DBlog.Entity
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string? Content { get; set; }
        public DateTime CommentDate { get; set; }
        public int UserId { get; set; }
        // public string? UserName { get; set; }
        public User User { get; set; } = null!;

        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;
        public List<Article> Articles { get; set; } = new List<Article>();


    }
}
