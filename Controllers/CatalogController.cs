using CityLibraryFund.Models;
using CityLibraryFund.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        private DatabaseContext _context;

        public CatalogController(DatabaseContext context)
        {
            _context = context;
        }
        //EDIT THIS METHOD
        [AllowAnonymous]
        public async Task<IActionResult> BookList(string genre, string searchResult, string flag, string sort, int page = 1)
        {
            IQueryable<Book> books;

            //filter and search
            if (String.IsNullOrEmpty(genre) || genre == "All")
            {
                if (!String.IsNullOrEmpty(searchResult))
                {
                    if (flag == "name")
                    {
                        books = _context.Books.Where(b => b.Name.Contains(searchResult));
                    }
                    else
                        books = _context.Books.Where(b => b.Author.Contains(searchResult));
                }
                else
                    books = _context.Books.OrderByDescending(b => b.Rating);
            }
            else if (!String.IsNullOrEmpty(genre) && genre != "All")
            {
                if (!String.IsNullOrEmpty(searchResult))
                {
                    if (flag == "name")
                    {
                        books = _context.Books.Where(b => b.Name.Contains(searchResult));
                    }
                    else
                        books = _context.Books.Where(b => b.Author.Contains(searchResult));
                }
                else
                    books = _context.Books.Where(b => b.Genre.Contains(genre));
            }
            else
                books = _context.Books.OrderByDescending(b => b.Rating);
            
            //sorting
            if (!String.IsNullOrEmpty(sort))
            {
                if (sort == "name")
                {
                    books = books.OrderByDescending(b => b.Name);
                }
                else if (sort == "year")
                {
                    books = books.OrderByDescending(b => b.Year);
                }
                else
                    books = books.OrderByDescending(b => b.Rating);
            }

            //paging
            int pageSize = 3;
            var count = await books.CountAsync();
            var items = await books.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            bool isUser = false;
            if (HttpContext.User.IsInRole("admin"))
            {
                isUser = true;
            }
            

            CatalogViewModel viewModel = new CatalogViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                Books = items,
                IsUser = isUser
            };
            return View(viewModel);
        }

        public async Task<IActionResult> AdministrateBooks()
        {
            return View(await _context.Copies.ToListAsync());
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id != null)
            {
                Book book = await _context.Books.FirstOrDefaultAsync(p => p.Id == id);
                //Copy copy = await _context.Copies.FirstOrDefaultAsync(p => p.BookId == id);
                //Library library = await _context.Libraries.FirstOrDefaultAsync(p => p.Id == copy.LibraryId);

                var temp = _context.Copies.Where(c => c.BookId == id).Select(u => new TempViewModel
                {
                    LibraryName = u.Library.Name,
                    LibraryId = u.LibraryId,
                    LibraryAddress = u.Library.Address
                });
                if (book != null && temp != null)
                {
                    string userName = HttpContext.User.Identity.Name;
                    User user = _context.Users.FirstOrDefault(u => u.Name == userName);
                    DetailViewModel viewModel = new DetailViewModel
                    {
                        Book = book,
                        //Copy = copy,
                        //Library = library,
                        User = user,
                        TempViews = temp.ToList()
                    };
                    return View(viewModel);
                }
                    
            }
            return NotFound();
        }

        public async Task<IActionResult> DetailBook(int? id)
        {
            if (id != null)
            {
                Book books = await _context.Books.FirstOrDefaultAsync(p => p.Id == id);
                if (books != null)
                    return View(books);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult InsertBook()
        {
            InsertBookViewModel viewModel = new InsertBookViewModel
            {
                maxIdBookValue = _context.Copies.Max(c => c.BookId),
                maxIdLibValue = _context.Copies.Max(c => c.LibraryId)
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> InsertBook(Copy copy)
        {
            Copy check = await _context.Copies.FirstOrDefaultAsync(b =>
                b.BookId == copy.BookId && b.LibraryId == copy.LibraryId);
            if (check == null)
            {
                _context.Copies.Add(copy);
                await _context.SaveChangesAsync();
            }
            else
            {
                check.Amount += copy.Amount;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("AdministrateBooks");
        }

        //for copies
        [HttpGet]
        public async Task<IActionResult> UpdateBook(int? id)
        {
            if (id != null)
            {
                Book books = await _context.Books.FirstOrDefaultAsync(p => p.Id == id);
                if (books != null)
                    return View(books);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBook(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("AdministrateBooks");
        }

        public async Task<IActionResult> DeleteBook(int? id)
        {
            if (id != null)
            {
                Book books = await _context.Books.FirstOrDefaultAsync(p => p.Id == id);
                if (books != null)
                {
                    _context.Books.Remove(books);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AdministrateBooks");
                }
            }
            return NotFound();
        }
    }
}
