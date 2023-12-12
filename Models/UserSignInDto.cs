using System.ComponentModel.DataAnnotations;

namespace Sklad.Models
{
    public class UserSignInDto
    {
        [Required(ErrorMessage = "Login is required.")]
        public string login { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string password { get; set; }
    }
}
