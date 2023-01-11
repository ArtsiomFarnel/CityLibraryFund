using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models.Entities
{
    public class Operation
    {
        [Key]
        public int Id { get; set; } //PK
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } //not null
        [Required]
        [MaxLength(50)]
        public string Description { get; set; }
        [Required]
        public DateTime DateAndTime { get; set; }

        public int UserId { get; set; } //FK
        public User User { get; set; } //navigation field

        public int BookId { get; set; } //FK
        public Book Book { get; set; } //navigation field
    }
}
