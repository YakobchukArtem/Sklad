﻿using Microsoft.AspNetCore.Mvc;
using Sklad.Models;
using System.Data.SqlClient;
using Newtonsoft.Json;


namespace Sklad.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult New_Product(int id)
        {
            Product product = new Product();
            if (id > 0)
            {
                product = DataBase.get(id)[0];
            }
            return View(product);
        }
        [HttpGet]
        public IActionResult Products()
        {
            return View("~/Views/Products/Products.cshtml", DataBase.get(0));
        }
        [HttpGet]
        public IActionResult Grid_Products()
        {
            return View("~/Views/Products/Grid_Products.cshtml", DataBase.get(0));
        }
        [HttpPost]
        public IActionResult Edit(Product model)
        {
            DataBase.add(model);
            return View("~/Views/Products/Products.cshtml", DataBase.get(0));
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
        static DataBase()
        {
            try
            {
                connectionString = File.ReadAllText("appsettings.secrets.json").Trim();
                System.Diagnostics.Debug.WriteLine(connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
            }
        }
        public static List<Product> get(int id)
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
                                product.id = reader.GetInt32(0);
                                product.name = reader.GetString(1);
                                product.category = reader.GetString(2);
                                product.producer = reader.GetString(3);
                                product.price = reader.GetDecimal(4);
                                product.count = reader.GetInt32(5);
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
    }
}