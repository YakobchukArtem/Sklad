using Sklad.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Office.Interop.Excel;

namespace Sklad
{
    public class DataBase
    {
        public static List<Product> listProducts = new List<Product>();
        private static readonly string connectionString;
        public static bool updateflag = false;
        public static string current_sort_parameter { get; set; }
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
        public static List<Product> get(int id, string parameter = null, string desc = null)
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
                                product.supplier = reader.GetString(6);
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
        public static void update(int id, Product model)
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
                        command.Parameters.AddWithValue("@name", model.name);
                        command.Parameters.AddWithValue("@category", model.category);
                        command.Parameters.AddWithValue("@producer", model.producer);
                        command.Parameters.AddWithValue("@price", model.price);
                        command.Parameters.AddWithValue("@count", model.count);
                        command.Parameters.AddWithValue("@supplier", string.IsNullOrEmpty(model.supplier) ? DBNull.Value : (object)model.supplier);
                        command.Parameters.AddWithValue("@measurement_unit", model.measurement_unit);
                        command.Parameters.AddWithValue("@price_unit", model.price_unit);
                        command.Parameters.Add("@product_image", SqlDbType.VarBinary, -1).Value = model.image != null && model.image.Length > 0 ? model.image : DBNull.Value;
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        public static List<String> get(string name)
        {
            string query = $"SELECT name FROM {name}";
            List<String> list = new List<String>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string categoryName = reader.GetString(0);

                            list.Add(categoryName);
                        }
                    }
                }
            }
            return list;
        }
        public static void add(string tablename, string name)
        {
            string query = $"INSERT INTO {tablename} VALUES ('{name}')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public static List<Product> Sample(string category = null, string producer = null, string supplier = null)
        {
            List<Product> productList = get(0);
            if (!string.IsNullOrEmpty(category))
                productList = productList.Where(p => p.category == category).ToList();

            if (!string.IsNullOrEmpty(producer))
                productList = productList.Where(p => p.producer == producer).ToList();

            if (!string.IsNullOrEmpty(supplier))
                productList = productList.Where(p => p.supplier == supplier).ToList();

            return productList;
        }
        public static User get_user(string login)
        {
            User user = null;

            string query = $"SELECT * FROM users WHERE login = '{login}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Login = reader["login"].ToString(),
                                Password = reader["password"].ToString(),
                                Name = reader["name"].ToString(),
                                IsLoggedIn = true
                            };
                        }
                    }
                }
            }

            return user;
        }

    }

}

