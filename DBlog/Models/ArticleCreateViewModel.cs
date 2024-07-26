using System.ComponentModel.DataAnnotations;

namespace DBlog.Models
{
    public class ArticleCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık gereklidir")]
        [StringLength(100, ErrorMessage = "Başlık 100 karakterden uzun olmamalıdır")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "İçerik gereklidir")]
        [StringLength(5000, ErrorMessage = "İçerik 5000 karakterden uzun olmamalıdır")]
        public string Content { get; set; } = null!;

        [Display(Name = "Makale Resmi")]
        public IFormFile? ImageFile { get; set; }
    }
}
