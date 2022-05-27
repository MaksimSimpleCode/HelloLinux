using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PictureAPI.Models
{
    public class Picture
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
        [Column(TypeName = "image")]
        public byte[] PictureData { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
