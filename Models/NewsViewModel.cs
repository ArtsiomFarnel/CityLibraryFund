using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models
{
    public class NewsViewModel
    {
        public IEnumerable<NewBooksViewModel> Books { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public bool IsUser { get; set; }
    }
}
