using HelloLinux.Infrastructure;
using HelloLinux.Models;
using HelloLinux.Services;
using HelloLinux.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        UserManager<User> _userManager;
        IUserRepository _userRepository;
        public PicturesController(PictureContext context, UserManager<User> userManager, IUserRepository userRepository)
        {
            db = context;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View(db.Pictures.ToList());
        }

        [HttpPost]
        public IActionResult Create(PictureViewModel model)
        {
            var userId = Guid.Parse(_userManager.GetUserId(this.User));
            //TODO добавить логгер
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
                    picture.Author = userId;

                    _userRepository.AddScore(userId, 1);

                    db.Pictures.Add(picture);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

    }
}
