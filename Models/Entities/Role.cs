using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; } //PK
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } //not null


        //users collection
        public List<User> Users { get; set; }
        public Role()
        {
            Users = new List<User>();
        }
    }
}
