namespace Sklad.Models
{
    public class Tables_data
    {
        public Tables_data(List<String> categories, List<String> suppliers, List<String> producers)
        {
            this.categories = categories;
            this.producers = producers;
            this.suppliers = suppliers;
        }
        public List<String> categories = new List<String>();
        public List<String> suppliers = new List<String>();
        public List<String> producers = new List<String>();
    }
}
