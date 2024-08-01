using System.ComponentModel.DataAnnotations;
using DBlog.Entity;
using Microsoft.AspNetCore.Http;

namespace DBlog.Models
{
    public class ArticleCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık gereklidir")]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "İçerik gereklidir")]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = null!;

        [Required]
        [Display(Name = "Url")]
        public string? Url { get; set; }

        public bool IsActive { get; set; }

        public string? ExistingImageFile { get; set; } // Mevcut görsel

        [Display(Name = "Makale Resmi")]
        public IFormFile? ImageFile { get; set; } // Yeni yüklenen görsel

        public List<Tag> Tags { get; set; } = new();

    }
}
