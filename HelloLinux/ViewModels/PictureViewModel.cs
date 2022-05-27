using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.ViewModels
{
    public class PictureViewModel
    {
        [Required(ErrorMessage ="Выберете картинку!")]
        public IFormFile Picture { get; set; }
    }
}
