using HelloLinux.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Controllers
{
    public class HomeController : Controller
    {
        private static Logger _logger = LogManager.GetLogger("Home_Logs");
        public HomeController()
        {

        }

        public IActionResult Index()
        {
            _logger.Info("Зашли на главную страницу Home");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
