using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using HelloLinux.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NLog;

namespace HelloLinux.Controllers
{
    public class RegistrationController:Controller
    {
        private static Logger _logger = LogManager.GetLogger("Registration_Logs");

        public RegistrationController()
        {
            
        }

        public IActionResult Registration()
        {
            _logger.Info("Мы зашли в Registration");
            return View();
        }

        public ViewResult CreateUser(User user)
        {
            
            if (ModelState.IsValid)
            {
                UserRepository.AddUser(user);

                _logger.Info($"Пользователь: {user.Login} зарегестрировался");
                return View("Thanks", user);
            }
            else
            {
                return View("Registration",user);
            }

        }
        public ViewResult ListUsers()
        {
            return View(UserRepository.Users.ToList());
        }

    }
}
