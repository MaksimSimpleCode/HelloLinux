using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux.Models
{
    public class User
    {
        [Required(ErrorMessage = "Пожалуйста, введите логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите пароль")]
        public string Password { get; set; }


    }
}
