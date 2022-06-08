using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using HelloLinux.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NLog;
using Microsoft.AspNetCore.Identity;
using HelloLinux.ViewModels;
using Microsoft.AspNetCore.Authorization;
using HelloLinux.Services;

namespace HelloLinux.Controllers
{

    public class AccountController : Controller
    {
        private static Logger _logger = LogManager.GetLogger("Registration_Logs");

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpGet]
        public async Task<IActionResult> PersonalArea()
        {
            var user = await _userManager.GetUserAsync(this.User);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditPersonalData(EditUserViewModel model)
        {
            //1 Сопсоб
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(this.User);
                user.UserName = model.Name;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.Year = model.Year;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("PersonalArea", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.Error(error.Description);
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return NoContent();
            
            //2 Способ
            // _userRepository.Edit(model);
            // return RedirectToAction("PersonalArea", "Account");

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
