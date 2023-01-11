using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models.Entities
{
    public class Copy
    {
        [Key]
        public int Id { get; set; } //PK
        [Required]
        public int Amount { get; set; }
        [Required]
        public DateTime ProductDate { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int LibraryId { get; set; }
        public Library Library { get; set; }
    }
}
