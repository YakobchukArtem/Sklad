using Microsoft.AspNetCore.Mvc;
using Sklad.Models;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Reflection.Metadata;


namespace Sklad.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult New_Product(int id)
        {
            DataBase.current_id= id;
            Product product = new Product();
            if (id > 0)
            {
                DataBase.updateflag = true;
                product = DataBase.get(id)[0];
            }
            return View(product);
        }
        [HttpGet]
        public IActionResult Products()
        {
            return View("~/Views/Products/Products.cshtml", DataBase.get(0, "count"));
        }
        [HttpGet]
        public IActionResult Grid_Products()
        {
            return View("~/Views/Products/Grid_Products.cshtml", DataBase.get(0));
        }
        [HttpPost]
        public IActionResult Edit(Product model)
        {
            if (DataBase.updateflag)
            {
                DataBase.update(DataBase.current_id, model);
                DataBase.updateflag = false;
            }
            else
            {
                DataBase.add(model);
            }
            return RedirectToAction("Products", DataBase.get(0));
        }
        public IActionResult Delete(int id)
        {
            DataBase.delete(id);
            return Json(new { success = true });
        }
    }

    public class DataBase
    {
        public static List<Product> listProducts = new List<Product>();
        private static readonly string connectionString;
        public static bool updateflag = false;
        public static int current_id { get; set; }
        static DataBase()
        {
            try
            {
                connectionString = File.ReadAllText("appsettings.secrets.json").Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
            }
        }
        public static List<Product> get(int id, string parameter = null, string desc=null)
        {
            listProducts.Clear();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sortOrder = (desc != null) ? "DESC" : "ASC";
                    string orderByClause = (!string.IsNullOrEmpty(parameter)) ? $"ORDER BY {parameter} {sortOrder}" : "";
                    string sql = $"SELECT * FROM Products {orderByClause}";
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
                                product.id = reader.GetInt32(0);
                                product.name = reader.GetString(1);
                                product.category = reader.GetString(2);
                                product.producer = reader.GetString(3);
                                product.price = reader.GetDecimal(4);
                                product.count = reader.GetInt32(5);
                                product.supplier = reader.IsDBNull(6) ? null : reader.GetString(6);
                                product.measurement_unit = reader.GetString(7);
                                product.price_unit = reader.GetString(8);
                                if (!reader.IsDBNull(9))
                                {
                                    byte[] imageData = (byte[])reader[9];
                                    product.image = imageData;
                                }
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
            return listProducts;
        }
        public static void add(Product model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Products" +
             "(name, category, producer, price, count, supplier, measurement_unit, price_unit, product_image) VALUES" +
             "(@name, @category, @producer, @price, @count, @supplier, @measurement_unit, @price_unit, @product_image);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", model.name);
                        command.Parameters.AddWithValue("@category", model.category);
                        command.Parameters.AddWithValue("@producer", model.producer);
                        command.Parameters.AddWithValue("@price", model.price);
                        command.Parameters.AddWithValue("@count", model.count);
                        command.Parameters.AddWithValue("@supplier", string.IsNullOrEmpty(model.supplier) ? DBNull.Value : (object)model.supplier);
                        command.Parameters.AddWithValue("@measurement_unit", model.measurement_unit);
                        command.Parameters.AddWithValue("@price_unit", model.price_unit);
                        command.Parameters.Add("@product_image", SqlDbType.VarBinary, -1).Value = model.image != null && model.image.Length > 0 ? model.image : DBNull.Value;
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        public static void delete(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "DELETE FROM Products WHERE ID = @Id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        public static void update(int id, Product updatedModel)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Products SET " +
                        "name = @name, " +
                        "category = @category, " +
                        "producer = @producer, " +
                        "price = @price, " +
                        "count = @count, " +
                        "supplier = @supplier, " +
                        "measurement_unit = @measurement_unit, " +
                        "price_unit = @price_unit, " +
                        "product_image = @product_image " +
                        "WHERE id = @id;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@name", updatedModel.name);
                        command.Parameters.AddWithValue("@category", updatedModel.category);
                        command.Parameters.AddWithValue("@producer", updatedModel.producer);
                        command.Parameters.AddWithValue("@price", updatedModel.price);
                        command.Parameters.AddWithValue("@count", updatedModel.count);
                        command.Parameters.AddWithValue("@supplier", updatedModel.supplier);
                        command.Parameters.AddWithValue("@measurement_unit", updatedModel.measurement_unit);
                        command.Parameters.AddWithValue("@price_unit", updatedModel.price_unit);
                        command.Parameters.AddWithValue("@product_image", updatedModel.image);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        public static List<Product> Sort(string parameter, string desc = null)
        {
            return get(0,parameter,desc);
        }
    }
}