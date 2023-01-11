using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models
{
    public class BestUsersViewModel
    {
        public IEnumerable<BestUsersResult> bestUsers;
        public bool IsUser { get; set; }
    }
}
