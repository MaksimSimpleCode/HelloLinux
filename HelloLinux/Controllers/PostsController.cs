using HelloLinux.Infrastructure;
using HelloLinux.Models;
using HelloLinux.Services;
using HelloLinux.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Controllers
{
    //  [Authorize]
    public class PostsController : Controller
    {
        private static Logger _logger = LogManager.GetLogger("Post_Logs");

        private readonly PostContext _db;
        private readonly ApplicationContext _dbUsr;
        UserManager<User> _userManager;
        IUserRepository _userRepository;
        public PostsController(PostContext context, ApplicationContext usrContext, IUserRepository userRepository, UserManager<User> userManager)
        {
            _db = context;
            _dbUsr = usrContext;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            #region Другой вариант Join
            //Оба варианта рабочие, за исключением того что нельзя делать Join с разным dbContext
            //1.

            //IQueryable<Post> items = _db.Posts.OrderBy(p => p.CreatedOn);
            //List<Post> allPosts = await items.ToListAsync();
            //return View(allPosts);


            //AllPosts allPosts = _db.Posts.Join(_dbUsr.Users,
            //    p => p.AuthorId.ToString(),
            //    u => u.Id,
            //    (p, u) => new AllPosts
            //    {
            //        Title = p.Title,
            //        Content = p.Content,
            //        CreatedOn = p.CreatedOn,
            //        Likes = p.Likes,
            //        AuthorName = u.UserName
            //    });

            //2.

            //IQueryable<AllPosts> all = from p in _db.Posts
            //                    join u in _dbUsr.Users on p.AuthorId.ToString() equals u.Id
            //            select new AllPosts  { Title=p.Title,Content=p.Content,CreatedOn=p.CreatedOn,Likes=p.Likes,AuthorName=u.UserName};
            //List<AllPosts> allPosts = await all.ToListAsync();
            #endregion

            //Такой колхоз пришлось шить из за того, что я пока не знаю как из разных dbContext сделать Join, возможно так нельзя.
            List<User> users = _dbUsr.Users.ToList();
            List<Post> posts = _db.Posts.ToList();

            IEnumerable<PostsViewModel> allPosts = from p in posts
                                                   join u in users on p.AuthorId.ToString() equals u.Id
                                                   orderby p.CreatedOn descending
                                                   select new PostsViewModel
                                                   {
                                                       Id = p.Id,
                                                       Title = p.Title,
                                                       Content = p.Content,
                                                       CreatedOn = p.CreatedOn,
                                                       Likes = p.Likes,
                                                       AuthorName = u.UserName
                                                   };

            return View(allPosts);
        }

        // GET /get/create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // GET /post/create
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            var userId = Guid.Parse(_userManager.GetUserId(this.User));

            Post post = new Post();
            if (ModelState.IsValid)
            {
                post.Title = model.Title;
                post.Content = model.Content;
                post.ModifiedOn = DateTime.Now;
                post.AuthorId = userId;
                post.Likes = 0;
                _db.Posts.Add(post);
                await _db.SaveChangesAsync();

                TempData["Sucess"] = "Пост добавлен!";
                _userRepository.AddScore(userId, 1);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MyPosts()
        {
            var userId = Guid.Parse(_userManager.GetUserId(this.User));
            List<User> users = _dbUsr.Users.ToList();
            List<Post> posts = _db.Posts.ToList();

            IEnumerable<PostsViewModel> myPosts = from p in posts
                                                  join u in users on p.AuthorId.ToString() equals u.Id
                                                  where p.AuthorId == userId && u.Id == userId.ToString()
                                                  orderby p.CreatedOn descending
                                                  select new PostsViewModel
                                                  {
                                                      Id = p.Id,
                                                      Title = p.Title,
                                                      Content = p.Content,
                                                      CreatedOn = p.CreatedOn,
                                                      Likes = p.Likes,
                                                      AuthorName = u.UserName
                                                  };
            return View(myPosts);
        }


        // GET /post/Edit
        public async Task<ActionResult> Edit(Guid id)
        {
            var userId = Guid.Parse(_userManager.GetUserId(this.User));

            Post post = await _db.Posts.FindAsync(id);
            if (post == null)
            {
                TempData["Error"] = "Записи не существует";
                return RedirectToAction("Index");
            }

            if (post.AuthorId != userId)
            {
                TempData["Error"] = "Доступ запрещен!";
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // POST /post/Edit
        [HttpPost]
     //   [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                post.ModifiedOn = DateTime.Now;

                _db.Posts.Update(post);
                await _db.SaveChangesAsync();

                TempData["Sucess"] = "Запись изменена";
                return RedirectToAction("Index");
            }
            return View(post);
        }


        // GET /get/InsidePost
        [HttpGet]
        public async Task<IActionResult> InsidePost(Guid id)
        {
            /*TODO InsidePost
             * 1. Сделать AuthorName у поста и комментария
             * 2. Сделать модальное окно для того, чтобы оставить комментарий
             * */


            Post post = await _db.Posts.FindAsync(id);
            List<Comment> comments = _db.Comments.Where(c => c.AuthorId == id).ToList();

            InsidePostViewModel model = new InsidePostViewModel { Post = post, Comments = comments };
            return View(model);
        }
    }
}
