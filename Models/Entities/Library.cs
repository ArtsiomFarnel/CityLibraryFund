using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models.Entities
{
    public class Library
    {
        [Key]
        public int Id { get; set; } //PK
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } //not null
        [Required]
        public string Description { get; set; }
        [Required]
        [MaxLength(50)]
        public string Address { get; set; }

        public List<Copy> Copies { get; set; }
        public Library()
        {
            Copies = new List<Copy>();
        }
    }
}
