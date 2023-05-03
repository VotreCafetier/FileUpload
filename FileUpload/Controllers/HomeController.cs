using FileUpload.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FileUpload.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        public HomeController(IWebHostEnvironment environment) => 
            hostingEnvironment = environment;

        public IActionResult Index() => View(new Post());

        [HttpPost]
        public IActionResult CreatePost(Post post)
        {
            if (post.MyImage == null) return Error();
            //convert name of file into yyyymmddHHSS (if already existing, add iterator)
            string uniqueFileName = GetUniqueFileName(post.MyImage.FileName);
            // upload file
            string uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
            string filePath = Path.Combine(uploads, uniqueFileName);
            FileStream newfile = new(filePath, FileMode.CreateNew);
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
        public IActionResult Error() => 
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}