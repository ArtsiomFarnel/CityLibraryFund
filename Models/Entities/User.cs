using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; } //PK
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } //not null
        [Required]
        [MaxLength(30)]
        public string Password { get; set; }
        [Required]
        [MaxLength(30)]
        public string Email { get; set; }
        [Required]
        public DateTime DateOfRegistration { get; set; }

        public int RoleId { get; set; } //FK
        public Role Role { get; set; } //navigation field
    }
}
