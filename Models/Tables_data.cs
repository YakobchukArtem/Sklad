using Sklad.Controllers;

namespace Sklad.Models
{
    public class Tables_data
    {
        public Tables_data(List<String> categories, List<String> suppliers, List<String> producers, string current_sort_parameter)
        {
            this.categories = categories;
            this.producers = producers;
            this.suppliers = suppliers;
            this.current_sort_parameter = current_sort_parameter;
        }
        public List<String> categories = new List<String>();
        public List<String> suppliers = new List<String>();
        public List<String> producers = new List<String>();
        public string current_sort_parameter;
    }
    public class Tables_data_methods
    {
        public static Tables_data get()
        {
            Tables_data tablesData = new Tables_data(
            DataBase.get("categories"),
            DataBase.get("producers"),
            DataBase.get("suppliers"),
            DataBase.current_sort_parameter
         );
            return tablesData;
        }
    }
}
