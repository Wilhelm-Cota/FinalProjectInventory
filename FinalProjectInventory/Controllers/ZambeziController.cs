using FinalProjectInventory.Data;
using FinalProjectInventory.Models;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml;
using Table = iText.Layout.Element.Table;

namespace FinalProjectInventory.Controllers
{
   
    public class ZambeziController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDBContext context;
        public ZambeziController(ApplicationDBContext context,IWebHostEnvironment environment)
        {
            this.context = context;
            this._environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RecordsAClass()
        {
            var results = context.AddRecordAClass.ToList();
            return View(results);
        }
        public IActionResult AddRecordA()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRecordA(AddRecordAClass model)
        {
            if (model.FileData == null || model.FileData.Length == 0)
            {
                ModelState.AddModelError("FileData", "Please select a file to upload.");
                return View(model);
            }
            
            // Define a directory where uploaded files will be stored
            var uploadsDirectory = Path.Combine(_environment.ContentRootPath, "Documents/");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }
            string folder = "Documents/";
            folder += Guid.NewGuid().ToString() + model.FileData.FileName;

            //file.ImagineUrl = folder  ### to insert the url into the database
            string serverFolder = Path.Combine(_environment.WebRootPath, folder);

            model.FileData.CopyTo(new FileStream(serverFolder, FileMode.Create));

            // Create a new entity to represent the data you want to store in the database
            var entity = new AddRecordAClass()
            {

                // Set the properties of the entity
                FilePath = folder,
                ProductId = model.ProductId,
                ProductDescription = model.ProductDescription,
                Quantity = model.Quantity,
                IssuedBy = model.IssuedBy,
                VerifiedBy = model.VerifiedBy,
                DateSend = model.DateSend,
                DateReceived = model.DateReceived
            };
            // Add the entity to the database
            context.AddRecordAClass.Add(entity);

            // Save changes to the database
            context.SaveChangesAsync();

            return RedirectToAction("RecordsAClass");
        }
        public IActionResult Download(int id, AddRecordAClass ad)
        {
            var result = context.AddRecordAClass.Where(m => ad.ProductId == id).FirstOrDefault();

            PdfWriter writer = new PdfWriter("C:\\Users\\Peter Helao\\Desktop\\paulus\\C#Code\\employee.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph header = new Paragraph("RecordS Report").SetTextAlignment(TextAlignment.CENTER).
                SetFontSize(20);

            Table table = new Table(7, true);
            Cell cell11 = new Cell(1, 1).SetBackgroundColor(ColorConstants.WHITE).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Product Id"));
            Cell cell12 = new Cell(1, 1).SetBackgroundColor(ColorConstants.WHITE).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Product Description"));
            Cell cell13 = new Cell(1, 1).SetBackgroundColor(ColorConstants.WHITE).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Quantity"));
            Cell cell14 = new Cell(1, 1).SetBackgroundColor(ColorConstants.WHITE).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Issued By"));
            Cell cell15 = new Cell(1, 1).SetBackgroundColor(ColorConstants.WHITE).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Verified By"));
            Cell cell16 = new Cell(1, 1).SetBackgroundColor(ColorConstants.WHITE).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Date Send"));
            Cell cell17 = new Cell(1, 1).SetBackgroundColor(ColorConstants.WHITE).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Date Recieved"));

            table.AddCell(cell11);
            table.AddCell(cell12);
            table.AddCell(cell13);
            table.AddCell(cell14);
            table.AddCell(cell15);
            table.AddCell(cell16);
            table.AddCell(cell17);

            foreach( var entity in result)
            {
                table.AddCell(entity.ProductId.ToString());
                table.AddCell(entity.ProductDescription);
                table.AddCell(entity.Quantity.ToString());
                table.AddCell(entity.IssuedBy);
                table.AddCell(entity.VerifiedBy);
                table.AddCell(entity.DateSend.ToString());
                table.AddCell(entity.DateReceived.ToString());
            }
            document.Add(table);

            // Close the PDF document and writer
            document.Close();
            writer.Close();

            byte[] fileBytes = System.IO.File.ReadAllBytes("C:\\Users\\Peter Helao\\Desktop\\paulus\\C#Code\\employee.pdf");
            return File(fileBytes, "application/pdf", "employee.pdf");
           // return RedirectToAction("RecordsAClass");
        }
    }
 
}
