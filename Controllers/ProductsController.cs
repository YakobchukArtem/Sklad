using Microsoft.AspNetCore.Mvc;
using Sklad.Models;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Data.Common;
using static iText.IO.Image.Jpeg2000ImageData;
using System.IO;


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
            ViewBag.Tables_data = Tables_data_methods.get();
            return View(product);
        }
        [HttpGet]
        public IActionResult Products(string parameter="ID", string desc = null)
        {
            bool shouldChangeParameter = (parameter == "Price" || parameter == "Count");
            string dynamicParameter = shouldChangeParameter ? $"{parameter}{(desc != null ? "1" : "2")}" : parameter;
            DataBase.current_sort_parameter = dynamicParameter;
            ViewBag.Tables_data = Tables_data_methods.get();
            return View("~/Views/Products/Products.cshtml", DataBase.get(0, parameter, desc));
        }


        [HttpGet]
        public IActionResult Grid_Products(string parameter = "ID", string desc = null)
        {
            bool shouldChangeParameter = (parameter == "Price" || parameter == "Count");
            string dynamicParameter = shouldChangeParameter ? $"{parameter}{(desc != null ? "1" : "2")}" : parameter;
            DataBase.current_sort_parameter = dynamicParameter;
            ViewBag.Tables_data = Tables_data_methods.get();
            return View("~/Views/Products/Grid_Products.cshtml", DataBase.get(0, parameter, desc));
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
        public IActionResult Sample(string category = null, string producer = null, string supplier = null)
        {
            return RedirectToAction("Products", DataBase.Sample(category, producer, supplier));
        }

        public IActionResult Report_PDF()
        {
            string filePath = "Report/report.pdf";
            Report.Print_Report_PDF(DataBase.get(0), filePath);
            return RedirectToAction("Products", DataBase.get(0));
        }
        public ActionResult DownloadFile()
        {
            string filePath = "Report/report.xlsx";
            Report.Print_Report_XLSX(DataBase.get(0), filePath);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", "report.xlsx");
        }
    }
}

