using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models.Entities
{
    public class Debt
    {
        [Key]
        public int Id { get; set; } //PK
        [Required]
        public DateTime FirstDate { get; set; }
        [Required]
        public DateTime LastDate { get; set; }
        public int PersonalRating { get; set; }
        public bool NotStarted { get; set; }
        public bool InProgress { get; set; }
        public bool IsFinished { get; set; }
        public bool Status { get; set; }

        public int UserId { get; set; } //FK
        public User User { get; set; } //navigation field

        public int BookId { get; set; } //FK
        public Book Book { get; set; } //navigation field

        public int LibraryId { get; set; } //FK
        public Library Library { get; set; } //navigation field
    }
}
