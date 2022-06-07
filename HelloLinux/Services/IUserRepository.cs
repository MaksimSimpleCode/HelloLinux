using HelloLinux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Services
{
    public interface IUserRepository
    {
        void AddScore(Guid id, int score);
        IEnumerable<User> GetTopUsers(int count);
    }
}
