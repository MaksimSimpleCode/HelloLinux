using Microsoft.EntityFrameworkCore;

namespace HelloLinux.Models
{
    public class PictureContext: DbContext
    {
        //TODO В дальнейшем картинки будут складываться на удаленную БД
        public DbSet<Picture> Pictures { get; set; }
        public PictureContext(DbContextOptions<PictureContext> options): base(options)
        {
            /*БД могу создать либо так либо применить миграцию на этот контекст (Update-Database -Context PictureContext)
             * это в случае если я меняю базу, переезжаю и прочее, и мне нужно воссоздать все базы.
             */

            //Database.EnsureCreated(); 
        }
    }
}
