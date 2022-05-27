using HelloLinux.Models;
using HelloLinux.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Controllers
{
    [Authorize]
    public class PicturesController : Controller
    {
        PictureContext db;

        public PicturesController(PictureContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View(db.Pictures.ToList());
        }

        [HttpPost]
        public IActionResult Create(PictureViewModel model)
        {
            if (ModelState.IsValid)
            {
                Picture picture = new Picture();
                //хз, лишняя ли проверка на null если используем валидацию?
                if (model.Picture != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(model.Picture.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Picture.Length);
                    }
                    // установка массива байтов
                    picture.PictureData = imageData;
                    picture.Name = model.Picture.FileName;
                    picture.CreatedOn = DateTime.Now;
                    db.Pictures.Add(picture);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

    }
}
