using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using OfficeOpenXml;
using Sklad.Models;

namespace Sklad
{
    public class Report
    {
        static Report()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public static void Print_Report_PDF(List<Product> products, string filePath)
        {
            using (var writer = new PdfWriter(filePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);
                    document.Add(new Paragraph("Product Report"));
                    Table table = new Table(8).UseAllAvailableWidth();
                    table.AddHeaderCell("Name");
                    table.AddHeaderCell("Category");
                    table.AddHeaderCell("Producer");
                    table.AddHeaderCell("Price");
                    table.AddHeaderCell("Count");
                    table.AddHeaderCell("Supplier");
                    table.AddHeaderCell("Measurement Unit");
                    table.AddHeaderCell("Price Unit");
                    foreach (var product in products)
                    {
                        table.AddCell(product.name);
                        table.AddCell(product.category);
                        table.AddCell(product.producer);
                        table.AddCell(product.price.ToString("C"));
                        table.AddCell(product.count.ToString());
                        table.AddCell(product.supplier);
                        table.AddCell(product.measurement_unit);
                        table.AddCell(product.price_unit);
                    }
                    document.Add(table);
                }
            }
        }
        public static void Print_Report_XLSX(List<Product> products, string filePath)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.Add("ProductReport");
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Category";
                worksheet.Cells[1, 3].Value = "Producer";
                worksheet.Cells[1, 4].Value = "Price";
                worksheet.Cells[1, 5].Value = "Count";
                worksheet.Cells[1, 6].Value = "Supplier";
                worksheet.Cells[1, 7].Value = "Measurement Unit";
                worksheet.Cells[1, 8].Value = "Price Unit";
                int row = 2;
                foreach (var product in products)
                {
                    worksheet.Cells[row, 1].Value = product.name;
                    worksheet.Cells[row, 2].Value = product.category;
                    worksheet.Cells[row, 3].Value = product.producer;
                    worksheet.Cells[row, 4].Value = product.price;
                    worksheet.Cells[row, 5].Value = product.count;
                    worksheet.Cells[row, 6].Value = product.supplier;
                    worksheet.Cells[row, 7].Value = product.measurement_unit;
                    worksheet.Cells[row, 8].Value = product.price_unit;
                    row++;
                }
                package.Save();
            }
        }
    }
}
