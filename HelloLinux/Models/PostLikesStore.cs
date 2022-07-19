using System;

namespace HelloLinux.Models
{
    public class PostLikesStore
    {
        public Guid Id { get; set; }
        public Guid AuthorID { get; set; }
        public Guid PostId { get; set; }
        public bool Like { get; set; }
    }
}
