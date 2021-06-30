using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SecuringApps_Synoptic.Models;

namespace SecuringApps_Synoptic.Controllers
{

    public class HomeController : Controller
    {
        public static string Key = "adef@@kfxcbv@";
        //private readonly IHostingEnvironment hostingEnvironment;
        private IHostingEnvironment Environment;
        private readonly ILogger<HomeController> _logger;


       

        public HomeController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }


        public async Task<IActionResult> IndexAsync(PasswordModel passwordModel)
        {
     
            string pass = passwordModel.Password;
           ConvertToEncrypt(passwordModel.Password);
           ConvertToDecrypt(ConvertToEncrypt(passwordModel.Password));

            ViewBag.EncryptedPassword = string.Format(ConvertToEncrypt(passwordModel.Password));
            ViewBag.DecryptedPassword = string.Format(ConvertToDecrypt(ConvertToEncrypt(passwordModel.Password)));

            if (ModelState.IsValid)
            {
                var formFile = passwordModel.Photo;
                if (formFile == null || formFile.Length == 0)
                {
                    ModelState.AddModelError("", "Uploaded file is empty or null.");
                    return View(viewName: "Index");
                }

                var uploadsRootFolder = Path.Combine(this.Environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }

                var filePath = Path.Combine(uploadsRootFolder, formFile.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream).ConfigureAwait(false);
                }

                RedirectToAction("Index");
            }

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> UploadPhoto(PasswordModel userViewModel)
        {
        
            return View(viewName: "Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
      
        public static string ConvertToEncrypt(string password)
        {
            if (string.IsNullOrEmpty(password)) return "";
            password += Key;
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }

        public static string ConvertToDecrypt(string base64EncodeData)
        {
            if (string.IsNullOrEmpty(base64EncodeData)) return "";
            var base64EncodeBytes = Convert.FromBase64String(base64EncodeData);
            var result = Encoding.UTF8.GetString(base64EncodeBytes);
            result = result.Substring(0, result.Length - Key.Length);
            return result;
        }

    }
}
