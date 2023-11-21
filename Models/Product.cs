namespace Sklad.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string producer { get; set; }
        public decimal price { get; set; }
        public int count { get; set; }
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
