using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBlog.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime PublishedDate { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; } // User ilişkisini ekledik

        public ICollection<Comment>? Comments { get; set; } // Comment ilişkisini ekledik
    }
}
