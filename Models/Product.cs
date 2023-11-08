namespace Sklad.Models
{
    public class Product
    {
        public string id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string producer { get; set; }
        public string price { get; set; }
        public  string count { get; set; }
        public string supplier { get; set; }
        public string measurement_unit { get; set; }    
        public string price_unit { get; set; }

    }

    public class Products_list : Product
    {
        public Products_list(List<Product> products_List)
        {
            this.listProducts = products_List;
        }
        public List<Product> listProducts = new List<Product>();
    }
}
