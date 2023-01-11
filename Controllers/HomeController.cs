using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CityLibraryFund.Models;
using Microsoft.AspNetCore.Authorization;
using CityLibraryFund.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Core.Objects;

namespace CityLibraryFund.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DatabaseContext _context;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        [Obsolete]
        public async Task<IActionResult> Index(int page = 1)
        {
            //add new on date
            //IQueryable<Book> books = _context.Books.Where(b => b.IsNew == true);
            
            var books_copies = (from c in _context.Copies
                         join b in _context.Books
                         on c.BookId equals b.Id
                         where (DateTime.Now.Day - c.ProductDate.Day) <= 10
                         select new NewBooksViewModel 
                         {
                             BookId = b.Id,
                             BookName = b.Name,
                             BookImage = b.Image,
                             BookAuthor = b.Author,
                             BookRating = b.Rating,
                             BookYear = b.Year,
                             CopyDateTime = c.ProductDate
                         });
            var temp = (from b in books_copies
                        group b by b.BookId into p
                        select new 
                        { 
                            p.Key
                        });
            var books = (from b in _context.Books
                         join t in temp
                         on b.Id  equals t.Key
                         select new NewBooksViewModel 
                         {
                             BookId = b.Id,
                             BookName = b.Name,
                             BookImage = b.Image,
                             BookAuthor = b.Author,
                             BookRating = b.Rating,
                             BookYear = b.Year,
                         });
            int pageSize = 2;
            var count = await books.CountAsync();
            var items = await books.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            NewsViewModel viewModel = new NewsViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                Books = items
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
