using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.ViewModels
{
    public class PostsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public int Likes { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
