using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Пустое поле!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пустое поле!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Неверный пароль!")]
        public string ConfirmPassword { get; set; }
    }
}
