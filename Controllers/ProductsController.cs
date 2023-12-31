﻿using Microsoft.AspNetCore.Mvc;
using Sklad.Models;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Data.Common;
using static iText.IO.Image.Jpeg2000ImageData;
using System.IO;
using System.Net.Mime;
using Microsoft.Net.Http.Headers;



namespace Sklad.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult New_Product(int id)
        {
            if(User_Name.Name != null)
            {
                ViewBag.Name = User_Name.Name;
                DataBase.current_id = id;
                Product product = new Product();
                if (id > 0)
                {
                    DataBase.updateflag = true;
                    product = DataBase.get(id)[0];
                }
                ViewBag.Tables_data = Tables_data_methods.get();
                return View(product);
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpGet]
        public IActionResult Products(string parameter="ID", string desc = null)
        {
            if (User_Name.Name != null)
            {
                bool shouldChangeParameter = (parameter == "Price" || parameter == "Count");
                string dynamicParameter = shouldChangeParameter ? $"{parameter}{(desc != null ? "1" : "2")}" : parameter;
                DataBase.current_sort_parameter = dynamicParameter;
                ViewBag.Tables_data = Tables_data_methods.get();
                ViewBag.Name = User_Name.Name;
                return View("~/Views/Products/Products.cshtml", DataBase.get(0, parameter, desc));
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }


        [HttpGet]
        public IActionResult Grid_Products(string parameter = "ID", string desc = null)
        {
            if (User_Name.Name != null)
            {
                bool shouldChangeParameter = (parameter == "Price" || parameter == "Count");
                string dynamicParameter = shouldChangeParameter ? $"{parameter}{(desc != null ? "1" : "2")}" : parameter;
                DataBase.current_sort_parameter = dynamicParameter;
                ViewBag.Tables_data = Tables_data_methods.get();
                ViewBag.Name = User_Name.Name;
                return View("~/Views/Products/Grid_Products.cshtml", DataBase.get(0, parameter, desc));
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpPost]
        public IActionResult Edit(Product model)
        {
            if (User_Name.Name != null)
            {
                ViewBag.Name = User_Name.Name;
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
            else
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public IActionResult Delete(int id)
        {
            if (User_Name.Name != null)
            {
                DataBase.delete(id);
                return Json(new { success = true });
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult DownloadFile()
        {
            if(User_Name.Name != null)
            {
                var filePath = "Report/report.xlsx";
                Report.Print_Report_XLSX(DataBase.get(0), filePath);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }

    }
}

