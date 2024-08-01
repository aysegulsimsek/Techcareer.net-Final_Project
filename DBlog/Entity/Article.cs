using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBlog.Entity
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık gereklidir")]
        [StringLength(100, ErrorMessage = "Başlık 100 karakterden uzun olmamalıdır")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "İçerik gereklidir")]
        public string Content { get; set; } = null!;
        public string? Url { get; set; }
        public bool IsActive { get; set; }
        public string? ImageFile { get; set; }
        // public string? ImageFilePath { get; set; }

        [Required(ErrorMessage = "Yayın tarihi gereklidir")]
        public DateTime PublishedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Kullanıcı ID'si gereklidir")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        // public List<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
