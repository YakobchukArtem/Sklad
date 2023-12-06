﻿using Microsoft.AspNetCore.Mvc;
using Sklad.Models;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Data.Common;
using static iText.IO.Image.Jpeg2000ImageData;


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
        public IActionResult Products(string parameter="id", string desc = null)
        {
            if(parameter!="id") DataBase.current_sort_parameter= parameter;
            ViewBag.Tables_data = Tables_data_methods.get();
            return View("~/Views/Products/Products.cshtml", DataBase.get(0, parameter, desc));
        }


        [HttpGet]
        public IActionResult Grid_Products(string parameter = "id", string desc = null)
        {
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
        public void Report_PDF()
        {
            string filePath = "Report/report.pdf";
            Report.Print_Report_PDF(DataBase.get(0), filePath);
        }
        public IActionResult Report_XLSX()
        {
            string filePath = "Report/report.xlsx";
            Report.Print_Report_XLSX(DataBase.get(0), filePath);
            return RedirectToAction("Products", DataBase.get(0));
        }
    }

}