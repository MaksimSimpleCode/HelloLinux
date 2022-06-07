using HelloLinux.Infrastructure;
using HelloLinux.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoContext _db;
        UserManager<User> _userManager;
        public ToDoController(ToDoContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        //TODO Поработать над фильтром записей
        public async Task<ActionResult> Index()
        {
            var userId = Guid.Parse(_userManager.GetUserId(this.User));
            IQueryable<ToDoList> items = _db.ToDoList.Where(item => item.UserId == userId).OrderBy(item => item.Id).Select(item=>item);
            List<ToDoList> toDoList = await items.ToListAsync();
            return View(toDoList);
        }

        // GET /todo/create
        public async Task<ActionResult> Create() => View();

        // POST /todo/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ToDoList item)
        {
            var userId = Guid.Parse(_userManager.GetUserId(this.User));
            if (ModelState.IsValid)
            {
                item.CreatedOn = DateTime.Now;
                item.ModifiedOn = DateTime.Now;
                item.UserId = userId;

                _db.ToDoList.Add(item);
                await _db.SaveChangesAsync();

                TempData["Sucess"] = "Запись добавлена";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET /todo/Edit
        public async Task<ActionResult> Edit(int id)
        {
            var userId = Guid.Parse(_userManager.GetUserId(this.User));

            ToDoList item = await _db.ToDoList.FindAsync(id);
            if(item == null)
            {
                TempData["Error"] = "Записи не существует";
            }

            if (item.UserId!=userId) {
                TempData["Error"] = "Доступ запрещен!";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // POST /todo/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ToDoList item)
        {
            if (ModelState.IsValid)
            {
                item.ModifiedOn = DateTime.Now;

                _db.ToDoList.Update(item);
                await _db.SaveChangesAsync();

                TempData["Sucess"] = "Запись изменена";
                return RedirectToAction("Index");
            }
            return View(item);
        }


        // GET /todo/delete
        public async Task<ActionResult> Delete(int id)
        {
            var userId = Guid.Parse(_userManager.GetUserId(this.User));

            ToDoList item = await _db.ToDoList.FindAsync(id);
            if (item == null)
            {
                TempData["Error"] = "Записи не существует";
            }

            if (item.UserId != userId)
            {
                TempData["Error"] = "Доступ запрещен!";
                return RedirectToAction("Index");
            }
            else
            {
                _db.ToDoList.Remove(item);
                await _db.SaveChangesAsync();

                TempData["Sucess"] = "Запись удалена!";
                return RedirectToAction("Index");
            }

            //return View(item);
        }
    }
}
