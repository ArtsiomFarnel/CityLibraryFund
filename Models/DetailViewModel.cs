using CityLibraryFund.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models
{
    public class DetailViewModel
    {
        public Book Book;
        //public Library Library;
        //public Copy Copy;
        public User User;
        public List<TempViewModel> TempViews;
    }
}
