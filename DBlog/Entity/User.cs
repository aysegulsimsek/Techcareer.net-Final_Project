using System.ComponentModel.DataAnnotations;

namespace DBlog.Entity
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; } = null!;

        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }


        public string? Image { get; set; }

        public bool IsAdmin { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
