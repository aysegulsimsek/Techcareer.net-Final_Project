using System.ComponentModel.DataAnnotations;

namespace DBlog.Models
{
    public class AddCommentViewModel
    {
        public int ArticleId { get; set; }
        public string? Content { get; set; }
    }
}
