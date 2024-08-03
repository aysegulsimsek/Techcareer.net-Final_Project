using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBlog.Entity
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Comment content is required")]
        [StringLength(500, ErrorMessage = "Content must be less than 500 characters")]
        public string Content { get; set; } = string.Empty;

        public DateTime CommentDate { get; set; } = DateTime.Now;

        [Required]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        [Required]
        public int ArticleId { get; set; }

        public Article Article { get; set; } = null!;


        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
