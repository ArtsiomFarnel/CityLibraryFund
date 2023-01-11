using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Пустое поле!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пустое поле!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
