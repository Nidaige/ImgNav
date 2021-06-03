using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpotNavigation.Data;
using SpotNavigation.Models;

namespace SpotNavigation.Controllers
{
    
    public class HomeController : Controller
    {
        private DraftDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DraftDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        [HttpPost]
        public IActionResult UploadImage()
        {
            string imgName = "";
            foreach(var file in Request.Form.Files)
            {
                imgName = file.FileName;
                Draft draft = new Draft();
                draft.ImageName = file.FileName;

                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                draft.ImageData = ms.ToArray();

                ms.Close();
                ms.Dispose();

                _db.Drafts.Add(draft);
                _db.SaveChanges();
                

            }
            var drafts = _db.Drafts.ToList();
            var Latest = drafts.First();
            string imageBase64Data = Convert.ToBase64String(Latest.ImageData);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewBag.ImageTitle = Latest.ImageName;
            ViewBag.ImageDataUrl = imageDataURL;
            return View("Index");
        }
    
        
    }
}