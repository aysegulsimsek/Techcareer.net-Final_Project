using System.ComponentModel.DataAnnotations;

namespace DBlog.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }

        public ICollection<Article>? Articles { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
