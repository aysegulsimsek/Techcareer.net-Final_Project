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
        public string? ImageFile { get; set; }
        public DateTime PublishedDate { get; set; }

        // User ilişkisini kaldırdık
        // public int UserId { get; set; }
        // public User? User { get; set; } 

        public ICollection<Comment>? Comments { get; set; } // Comment ilişkisini koruduk
    }
}