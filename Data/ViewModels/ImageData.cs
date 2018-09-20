using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Data.ViewModels
{
    public class ImageData
    {
        [Required]
        [Display(Name = "ImageFile")]
        public List<IFormFile> ImageFile { get; set; }
    }
}
