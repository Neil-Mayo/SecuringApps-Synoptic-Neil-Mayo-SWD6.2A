using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecuringApps_Synoptic.Models
{

    public class PasswordModel
    {
        public string Password { get; set; }

        public string PhotoPath { get; set; }

       

        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(10 * 1024 * 1024)]
        [AllowBMP(new string[] { ".bmp" })]
        public IFormFile Photo { get; set; }
    }
}
