using HelloLinux.Models;
using HelloLinux.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private static Logger _logger = LogManager.GetLogger("Home_Logs");
        IUserRepository _userRepository;
        public HomeController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            _logger.Info("Зашли на главную страницу Home");
            var topUsers=_userRepository.GetTopUsers(5);
            return View(topUsers);
        }
    }
}
