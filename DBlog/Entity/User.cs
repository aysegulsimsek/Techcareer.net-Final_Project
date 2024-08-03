using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBlog.Entity
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string? UserName { get; set; }

        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Password { get; set; }
        public string? Image { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }




        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
