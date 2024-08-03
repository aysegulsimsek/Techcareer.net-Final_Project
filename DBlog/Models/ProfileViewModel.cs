using DBlog.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBlog.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; } = null!;
        public int UserId { get; set; }
        public string? UserName { get; set; } = null!;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public List<User> Users { get; set; } = new();
        public List<Article> Articles { get; set; } = new();

        public List<Comment> Comments { get; set; } = new();

    }
}
