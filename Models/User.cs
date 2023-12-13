
namespace Sklad.Models
{
    public class User
    {
        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool IsLoggedIn { get; set; }
    }
    public class User_Name
    {
        public static string Name { get; set; }
    }
}
