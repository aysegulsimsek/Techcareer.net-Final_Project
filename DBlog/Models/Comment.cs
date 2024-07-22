using System;
using System.ComponentModel.DataAnnotations;

namespace DBlog.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string? Content { get; set; }
        public DateTime CommentDate { get; set; }

        public int ArticleId { get; set; }
        public Article? Article { get; set; } // Burayı 'required' yerine 'Article?' olarak değiştirdik
        public int UserId { get; set; }
        public User? User { get; set; } // User ilişkisini ekledik
    }
}
