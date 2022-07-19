using HelloLinux.Infrastructure;
using HelloLinux.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Controllers
{
    public class PostsController : Controller
    {
        private static Logger _logger = LogManager.GetLogger("Post_Logs");

        private readonly PostContext _db;
        UserManager<User> _userManager;
        public PostsController(PostContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
          
        }
    }
}
