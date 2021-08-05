using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            foreach (var file in Request.Form.Files)
            {
                Draft draft = new Draft();
                draft.ImageName = file.FileName;
                MemoryStream ms = new MemoryStream();
                string fileExtension = file.FileName.Substring(file.FileName.Length - 3, 3);
                if (fileExtension == "pdf") // If file has pdf extension, convert it to png filestream
                {
                    MemoryStream imageStream = new MemoryStream();
                    file.CopyTo(imageStream);
                    var pdf = new Aspose.Pdf.Document(imageStream);
                    ms = new Aspose.Pdf.Document(imageStream).ConvertPageToPNGMemoryStream(pdf.Pages[1]);
                }
                else if (fileExtension == "png" // if it has extension png, just copy to stream
                )
                {
                    file.CopyTo(ms);
                }
                else // Not a supported format, return nothing
                {
                    return View("Index");
                    
                }
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
            var Paths = _db.Paths.ToList();
            if (Paths.Count != 0)
            {
                _db.Remove(Paths.Last());
            }

            _db.Paths.Add(solutionString);
            _db.SaveChanges();
            //@ViewBag.SolutionString = solutionString;
            return View(solutionString);
        }

        [HttpGet]
        public IActionResult SendSolution()
        {
            Solution returnPath = new Solution();
            returnPath.SolString = "No solution in database";
            var Paths = _db.Paths.ToList();
            if (Paths.Count != 0)
            {
                returnPath.SolString = Paths.First().SolString;
            }
            return View(returnPath);
        }


    }
}