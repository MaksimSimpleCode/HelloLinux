using Microsoft.EntityFrameworkCore;

namespace HelloLinux.Models
{
    public class PictureContext: DbContext
    {
        //TODO В дальнейшем картинки будут складываться на удаленную БД
        public DbSet<Picture> Pictures { get; set; }
        public PictureContext(DbContextOptions<PictureContext> options): base(options)
        {
            Database.EnsureCreated();
        }
    }
}
