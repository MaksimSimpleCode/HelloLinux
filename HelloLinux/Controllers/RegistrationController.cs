using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using HelloLinux.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HelloLinux.Controllers
{
    public class RegistrationController:Controller
    {
        private readonly ILogger<HomeController> _logger;
        public RegistrationController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Registration()
        {
            return View();
        }

        public ViewResult CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                UserRepository.AddUser(user);
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
