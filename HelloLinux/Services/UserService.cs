using HelloLinux.Infrastructure;
using HelloLinux.Models;
using HelloLinux.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Services
{
    public class UserService : IUserRepository
    {
        private readonly ApplicationContext _db;
        public UserService(ApplicationContext context)
        {
            _db = context;
        }

        public void AddScore(Guid id, int score)
        {
            User user = _db.Users.FirstOrDefault(user => user.Id == id.ToString()) ;
            if (user != null)
            {
                user.Score += score;
                _db.SaveChanges();
            }
        }

        public void Edit(EditUserViewModel model)
        {
            User user = _db.Users.FirstOrDefault(user => user.Id == model.Id.ToString());
            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.Name;
                user.PhoneNumber = model.PhoneNumber;
                user.Year = model.Year;
                _db.SaveChanges();
            }
        }

        public IEnumerable<User> GetTopUsers(int count)
        {
            var topUsers = _db.Users.Take(count).OrderByDescending(user => user.Score).ToList();
            return topUsers;
        }
    }
}
