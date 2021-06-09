using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private String DisplayCanvas;

        public HomeController(ILogger<HomeController> logger, DraftDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.DisplayCanvas = "none";
            ViewBag.DisplayForm = "block";
            ViewBag.BtnVis = "none";
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
                var drafts2 = _db.Drafts.ToList();
                if (drafts2.Count!=0)
                {
                    var Latest2 = drafts2.Last();
                    _db.Remove(Latest2);
                    _db.SaveChanges();
                }
                _db.Drafts.Add(draft);
                _db.SaveChanges();
            }
            var drafts = _db.Drafts.ToList();
            var Latest = drafts.Last();
            string imageBase64Data = Convert.ToBase64String(Latest.ImageData);
            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            ViewBag.ImageTitle = Latest.ImageName;
            ViewBag.ImageDataUrl = imageDataURL;
            ViewBag.DisplayCanvas = "flex";
            ViewBag.DisplayForm = "none";
            ViewBag.BtnVis = "inline";

            return View("Index");
        }
        
        [HttpPost]
        public IActionResult SendSolution(Solution solutionString)
        {
            //@ViewBag.SolutionString = solutionString;
            return View(solutionString);
        }
    
        
    }
}