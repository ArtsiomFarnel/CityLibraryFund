using CityLibraryFund.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models
{
    public class DebtsViewModel
    {
        public string BookName { get; set; }
        public string BookImage { get; set; }
        public string BookAuthor { get; set; }
        public int BookRating { get; set; }
        public int BookYear { get; set; }
        public int BookId { get; set; }
        public int LibId { get; set; }
        public List<Copy> Copies { get; set; }
    }
}
