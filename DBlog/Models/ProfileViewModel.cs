using DBlog.Entity;

namespace DBlog.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; } = null!;
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
