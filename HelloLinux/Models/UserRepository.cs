using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Models
{
    public class UserRepository
    {
        public static List<User> users = new List<User>();

        public static IEnumerable<User> Users
        {
            get { return users; }
        }
        public static void AddUser(User user)
        {
            users.Add(user);
        }
    }
}
