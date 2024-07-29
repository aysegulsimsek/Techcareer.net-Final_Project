// Models/ContactViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace DBlog.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message cannot be longer than 500 characters")]
        public string? Message { get; set; }
    }
}
