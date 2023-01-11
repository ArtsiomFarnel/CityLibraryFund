using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; } //PK
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } //not null
        [Required]
        public string Description { get; set; }
        //[StringLength(4)] //НЕ ИСПОЛЬЗОВАТЬ! ВЫЛЕТАЕТ ОШИБКА КАСТА
        public int Year { get; set; }
        [MaxLength(100)]
        public string Genre { get; set; }
        [MaxLength(50)]
        public string Author { get; set; }
        public string Image { get; set; }
        public bool IsNew { get; set; }
        public int Rating { get; set; }

        public List<Copy> Copies { get; set; }
        public Book()
        {
            Copies = new List<Copy>();
        }
    }
}
