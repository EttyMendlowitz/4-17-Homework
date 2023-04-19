using _417Homework.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Diagnostics;

namespace _417Homework.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile image, string password)
        {
            string fileName = $"{Guid.NewGuid()} - {image.FileName}";
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using FileStream fs = new FileStream(filePath, FileMode.CreateNew);
            image.CopyTo(fs);

            ImageManager im = new ImageManager();
            int id = im.AddImage(new Image
            {
                FileName = fileName,
                Password = password,
                Views = 1
            });

            return Redirect($"/home/uploads?id={id}");
        }

        public IActionResult Uploads(int id)
        {
            ImageViewModel ivm = new ImageViewModel();
            ImageManager im = new ImageManager();
            ivm.Image = im.GetImageForId(id);
            return View(ivm);
        }

        public IActionResult ViewImage(int id)
        {
            List<int> ids = HttpContext.Session.Get<List<int>>("id");
            if (ids != null && ids.Contains(id) )
            {
                return Redirect($"/home/ImagePage?id={id}");
            }

            ImageManager im = new ImageManager();
            ImageViewModel ivm = new ImageViewModel();
            ivm.Image = im.GetImageForId(id);
            return View(ivm);
        }

        public IActionResult CheckPassword(int id, string password)
        {
            TempData["Message"] = null;
            ImageManager im = new ImageManager();
            Image image = im.GetImageForId(id);

            if (image.Password.Trim() == password.Trim())
            {
                List<int> ids = HttpContext.Session.Get<List<int>>("id");
                if (ids == null)
                {
                    ids = new List<int>();
                }
                ids.Add(id);
                HttpContext.Session.Set<List<int>>("id", ids);
                return Redirect($"/home/ImagePage?id={id}");
            }

            TempData["Message"] = "Invalid password!";
            return Redirect($"/home/ViewImage?id={id}");


        }

        public IActionResult ImagePage(int id)
        {
            ImageViewModel ivm = new ImageViewModel();
            ImageManager im = new ImageManager();
            Image image = im.GetImageForId(id);
            ivm.Image = image;
            im.IncreaseViews(id, image.Views + 1);
            return View(ivm);
        }



    }
}