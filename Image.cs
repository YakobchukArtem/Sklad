using System.Data.SqlClient;

namespace Sklad
{
    public class Image
    {
        public static byte[] ReadImageBytesFromFile(string imagePath)
        {
            byte[] imageBytes;

            try
            {
                imageBytes = File.ReadAllBytes(imagePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading image bytes: {ex.Message}");
                imageBytes = null;
            }

            return imageBytes;
        }

    }
}
