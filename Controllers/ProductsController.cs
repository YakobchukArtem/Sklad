using Microsoft.AspNetCore.Mvc;
using Sklad.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;

namespace Sklad.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult New_Product(int id)
        {
            Product product = new Product();
            if (id > 0)
            {
                DataBase.get(id);
                product = DataBase.listProducts[0];
            }
            return View(product);
        }
        public IActionResult Products()
        {
            Edit();
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Product model)
        {
            DataBase.post(model);
            return View("New_Product"); 
        }
        [HttpGet]
        public IActionResult Edit()
        {
            DataBase.get(0);
            Products_list model = new Products_list(DataBase.listProducts); 
            return View("~/Views/Products/Products.cshtml", model);
        }
    }

    public class DataBase
    {
        public static List<Product> listProducts = new List<Product>();
        private static string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Sklad;Integrated Security=True";
        public static void get(int id)
        {
            listProducts.Clear();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "SELECT * FROM Products";
                    connection.Open();
                    if (id > 0)
                    {
                        sql = $"SELECT * FROM Products WHERE id = {id}";
                    }                   
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product();
                                product.id = "" + reader.GetInt32(0);
                                product.name = reader.GetString(1);
                                product.category = reader.GetString(2);
                                product.producer = reader.GetString(3);
                                product.price = reader.GetDecimal(4).ToString(); 
                                product.count = reader.GetInt32(5).ToString();
                                product.supplier = reader.GetString(6);
                                product.measurement_unit = reader.GetString(7);
                                product.price_unit = reader.GetString(8);
                                listProducts.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }     
        }
        public static void ClearList()
        {
            listProducts.Clear();
        }
        public static void post(Product model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Products" +
                        "(name, category, producer, price, count, supplier, measurement_unit, price_unit) VALUES" +
                     "(@name, @category, @producer, @price, @count, @supplier, @measurement_unit, @price_unit);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", model.name);
                        command.Parameters.AddWithValue("@category", model.category);
                        command.Parameters.AddWithValue("@producer", model.producer);
                        command.Parameters.AddWithValue("@price", model.price);
                        command.Parameters.AddWithValue("@count", model.count);
                        command.Parameters.AddWithValue("@supplier", model.supplier);
                        command.Parameters.AddWithValue("@measurement_unit", model.measurement_unit);
                        command.Parameters.AddWithValue("@price_unit", model.price_unit);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
