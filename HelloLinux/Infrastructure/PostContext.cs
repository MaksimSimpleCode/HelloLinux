using HelloLinux.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Infrastructure
{
    public class PostContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostLikesStore> PostLikesStores { get; set; }
        public PostContext(DbContextOptions<PostContext> options):base(options)
        {
        }
    }
}
