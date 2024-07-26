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
        [StringLength(5000, ErrorMessage = "İçerik 5000 karakterden uzun olmamalıdır")]
        public string Content { get; set; } = null!;

        public string? ImageFile { get; set; }

        [Required(ErrorMessage = "Yayın tarihi gereklidir")]
        public DateTime PublishedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Kullanıcı ID'si gereklidir")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
