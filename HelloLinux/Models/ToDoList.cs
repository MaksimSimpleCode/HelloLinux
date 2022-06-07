using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Models
{
    public class ToDoList
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
