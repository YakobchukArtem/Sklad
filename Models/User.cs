using System.ComponentModel.DataAnnotations;

namespace Sklad.Models
{
    public class User
    {
        public string Name { get; set; }

        [Required(ErrorMessage = "Login is required.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public bool IsLoggedIn { get; set; }
    }
}
