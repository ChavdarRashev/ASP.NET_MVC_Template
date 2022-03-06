using FileManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FileManagement.Utilities;
using FileManagement.Data;
using Microsoft.Extensions.Configuration;
using FileManagement.Models.ViewModels;
using DataCommon.Model;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace FileManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        private readonly FileDBContext _context;
       

        

        public HomeController(ILogger<HomeController> logger, IConfiguration config, FileDBContext context)
        {
            _logger = logger;
            _config = config;
            _context = context;
        }

        
        public IActionResult Index()
        {
            int maxFileSize = _config.GetValue<int>("FileUploadsParameter:MaxFileSizeMB");
            string[] permittedExtensions = _config.GetSection("FileUploadsParameter:PermittedExtensions").Get<List<string>>().ToArray();
            string fileTypes = String.Join(", ", permittedExtensions);

            ViewBag.message = $"Файлът трябва да е не по-голям от {maxFileSize:N1} MB и да е от тип(разширение) {fileTypes}";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(InputFileModel FileUpload)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Result = "Грешни данни във формата.";

                return View("Index");
            }

            int maxFileSizeMB = _config.GetValue<int>("FileUploadsParameter:MaxFileSizeMB");
            var byteSizeLimit = maxFileSizeMB * 1048576;
            string[] permittedExtensions = _config.GetSection("FileUploadsParameter:PermittedExtensions").Get<List<string>>().ToArray();

            // string[] permittedExtensions = { ".txt" };
            var formFileContent =
                await FileHelpers.ProcessFormFile<InputFileModel>(
                    FileUpload.FormFile, ModelState, permittedExtensions,
                    byteSizeLimit);

            // Perform a second check to catch ProcessFormFile method
            // violations. If any validation check fails, return to the
            // page.
            if (!ModelState.IsValid)
            {
                ViewBag.Result = "Грешни данни във формата.";

                return View("Index");
            }

            //Всеки качен файл в базата данни се записва и с ново алучйно име. При виуализация (сваляне) на този файл от потребителския интерфейс се използва това ново име.
            string newRandomFileName = FileHelpers.NewRandomFileName(FileUpload.FormFile.FileName);
            
            FileInfo fi = new FileInfo(FileUpload.FormFile.FileName);

            var file = new FileRashev
            {
                Content = formFileContent,
                UntrustedName = FileUpload.FormFile.FileName,
                Size = FileUpload.FormFile.Length,
                NewTrustedName = newRandomFileName,
                MIMEtype = FileHelpers.GetMIMEType(fi.Extension)
            };

            try
            {
                if (this.ModelState.IsValid)
                {
                    this._context.Add(file);
                    await this._context.SaveChangesWithDateTimeAsync();
                    return this.RedirectToAction(nameof(this.Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                // Log the error (uncomment ex variable name and write a log.
                this.ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

           
            return RedirectToPage("./Index");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        //Тук Task-a принципно трябва да е от тип FileResult, но FileResult не може да връща NotFound,а раизва грешка 500, което е по-неприемливо.
        // Много често не се подава id параметър или се подава несъщесвуващо id и по-добре е да се върне NotFound,а не да се визуализра грешка при потребителя
        // Поради тази причина Task е от тип <IActionResult>
        public async Task<IActionResult> Download(int id)
        {
            if (id == 0) { return NotFound(); }
            FileRashev downFile = new FileRashev();
            
            downFile =await _context.Files.Where(a => a.Id == id).SingleOrDefaultAsync();
            if (downFile == null) { return NotFound(); }
            
            return File(downFile.Content, downFile.MIMEtype, downFile.NewTrustedName);
           
           
        }

        public async Task<FileResult> Picture(int id)
        {
            if (id == 0) { return null; }
            FileRashev downFile = new FileRashev();

            downFile = await _context.Files.Where(a => a.Id == id).SingleOrDefaultAsync();
            // Response.AppendHeader("content-disposition", "inline; filename=file.pdf"); //this will open in a new tab.. remove if you want to open in the same tab.
            return File(downFile.Content, downFile.MIMEtype, downFile.NewTrustedName);
            // return File(memory, GetContentType(path), Path.GetFileName(path));

        }

        //https://andrewlock.net/re-execute-the-middleware-pipeline-with-the-statuscodepages-middleware-to-create-custom-error-pages/
        //http://www.binaryintellect.net/articles/2f55345c-1fcb-4262-89f4-c4319f95c5bd.aspx

        public IActionResult StatusCode (string code)
        {
            return View(code);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
