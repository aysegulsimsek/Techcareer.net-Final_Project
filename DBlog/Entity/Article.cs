using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBlog.Entity
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImageFile { get; set; }
        public DateTime PublishedDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Comment>? Comments { get; set; }
    }
}