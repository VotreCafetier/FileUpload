using FileUpload.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FileUpload.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IWebHostEnvironment hostingEnvironment;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            hostingEnvironment = environment;
        }

        public IActionResult Index() => View(new Post());

        [HttpPost]
        public IActionResult CreatePost(Post post)
        {
            if (post.MyImage == null) return Error();
            //convert name of file into yyyymmddHHSS (if already existing, add iterator)
            var uniqueFileName = GetUniqueFileName(post.MyImage.FileName);
            // upload file
            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, uniqueFileName);
            var newfile = new FileStream(filePath, FileMode.CreateNew);
            post.MyImage.CopyTo(newfile); 
            newfile.Close();

            return RedirectToAction("Index", "Home");
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}