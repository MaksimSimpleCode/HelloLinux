using HelloLinux.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Infrastructure
{
    public class ToDoContext:DbContext
    {
        public DbSet<ToDoList> ToDoList { get; set; }
        public ToDoContext(DbContextOptions<ToDoContext> options):base(options)
        {
        }
    }
}
