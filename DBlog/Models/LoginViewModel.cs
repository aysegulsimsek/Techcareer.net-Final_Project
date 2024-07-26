using System.ComponentModel.DataAnnotations;
using DBlog.Entity;

namespace DBlog.Models
{

    public class LoginViewModel
    {

        [Required]
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "{0} alanı en az {2} an çok {1} karakter uzunluğunda olmalıdır.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string? Password { get; set; }
    }
}