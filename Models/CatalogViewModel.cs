using CityLibraryFund.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models
{
    public class CatalogViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public bool IsUser { get; set; }
    }
}
