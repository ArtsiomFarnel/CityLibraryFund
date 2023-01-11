using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityLibraryFund.Models;
using CityLibraryFund.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityLibraryFund.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private DatabaseContext _context;

        public ClientsController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Profile()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userName = HttpContext.User.Identity.Name;
                User user = _context.Users.FirstOrDefault(u => u.Name == userName);
                return View(user);
            }
            else
                return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> ClientsBooks(int? id)
        {
            var clientsBooks = _context.Debts.Where(d => d.UserId == id && d.Status == true).Select(p => new DebtsViewModel
            {
                BookName = p.Book.Name,
                BookId = p.Book.Id,
                BookAuthor = p.Book.Author,
                BookImage = p.Book.Image,
                BookRating = p.Book.Rating,
                BookYear = p.Book.Year,
                //Copies = p.Book.Copies.Where(c => c.BookId == p.Book.Id).ToList(),
                //add libId
                LibId = p.LibraryId
            });
            
            return View(await clientsBooks.ToListAsync());
        }

        public async Task<IActionResult> TakeBook(int? userId, int? bookId, int? libraryId)
        {
            //List<Debt> debts = await _context.Debts.Where(d => d.Status == true && d.BookId == (int)bookId).ToListAsync();
            if (_context.Debts.Any(d => d.Status == true && d.BookId == (int)bookId && d.UserId == (int)userId && d.LibraryId == (int)libraryId))
            {
                return RedirectToAction("BookList", "Catalog");
            }

            TakeBookViewModel viewModel = new TakeBookViewModel
            {
                Operation = new Operation
                {
                    Name = "take",
                    DateAndTime = DateTime.Now,
                    Description = "Some shit",
                    UserId = (int)userId,
                    BookId = (int)bookId
                },
                Debt = new Debt
                {
                    UserId = (int)userId,
                    BookId = (int)bookId,
                    LibraryId = (int)libraryId,
                    FirstDate = DateTime.Now,
                    LastDate = DateTime.Now.AddDays(10),
                    Status = true
                }
            };
            Copy copy = _context.Copies.Where(c => c.BookId == (int)bookId && c.LibraryId == (int)libraryId).FirstOrDefault();
            copy.Amount -= 1;
            _context.Operations.Add(viewModel.Operation);
            _context.Debts.Add(viewModel.Debt);
            await _context.SaveChangesAsync();
            //return View(viewModel);
            return RedirectToAction("BookList", "Catalog");
        }

        public async Task<IActionResult> ReturnBook(int? bookId, int? libraryId)
        {
            string userName = HttpContext.User.Identity.Name;
            User user = _context.Users.FirstOrDefault(u => u.Name == userName);

            Operation operation = _context.Operations.Where(o => o.BookId == (int)bookId && o.UserId == user.Id && o.Name == "take").FirstOrDefault();
            operation.Name = "return";

            Debt debt = _context.Debts.Where(d => d.BookId == (int)bookId && d.UserId == user.Id && d.Status == true).FirstOrDefault();
            debt.Status = false;

            Copy copy = _context.Copies.Where(c => c.BookId == (int)bookId && c.LibraryId == (int)libraryId).FirstOrDefault();
            copy.Amount += 1;

            await _context.SaveChangesAsync();
            //return View();
            //return RedirectToAction("ClientsBooks", "Clients");
            //route values
            User routeId = _context.Users.FirstOrDefault(u => u.Name == userName);
            return RedirectToAction("ClientsBooks", "Clients", new { routeId.Id });
        }
        [HttpGet]
        public async Task<IActionResult> SetRating(int? bookId, int? libId)
        {
            string userName = HttpContext.User.Identity.Name;
            User user = _context.Users.FirstOrDefault(u => u.Name == userName);

            Debt debt = await _context.Debts.FirstOrDefaultAsync(b => b.BookId == (int)bookId && b.LibraryId == (int)libId && b.UserId == user.Id);
            if (debt != null)
            {
                return View(debt);
            }
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> SetRating(int? bookId, int? libId, int? rating, bool? isFinished)
        {
            string userName = HttpContext.User.Identity.Name;
            User user = _context.Users.FirstOrDefault(u => u.Name == userName);

            Debt debt = await _context.Debts.FirstOrDefaultAsync(b => b.BookId == (int)bookId && b.LibraryId == (int)libId && b.UserId == user.Id);
            debt.PersonalRating = (int)rating;
            debt.IsFinished = (bool)isFinished;
            await _context.SaveChangesAsync();
            
            Book book = await _context.Books.FirstOrDefaultAsync(p => p.Id == (int)bookId);
            book.Rating = (int) await _context.Debts.Where(c => c.BookId == (int)bookId).AverageAsync(u => u.PersonalRating);
            await _context.SaveChangesAsync();

            //login
            //return RedirectToAction("Index", "Home");
            //route values
            User routeId = _context.Users.FirstOrDefault(u => u.Name == userName);
            return RedirectToAction("ClientsBooks", "Clients", new { routeId.Id });
        }

        [AllowAnonymous]
        public IActionResult BestUsers()
        {
            bool isUser = false;
            if (HttpContext.User.IsInRole("admin"))
            {
                isUser = true;
            }

            var users = (from d in _context.Debts
                         join u in _context.Users
                         on d.UserId equals u.Id
                         where d.IsFinished == true
                         group d by d.UserId into p
                         select new BestUsersType
                         {
                            Id = p.Key,
                            Count = p.Count()
                         });
            users.OrderByDescending(u => u.Count);
            var bestUsers = (from b in _context.Users
                             join u in users
                             on b.Id equals u.Id
                             select new BestUsersResult
                             {
                                 Name = b.Name,
                                 Count = u.Count
                             });
            BestUsersViewModel viewModel = new BestUsersViewModel
            {
                bestUsers = bestUsers.OrderByDescending(u => u.Count),
                IsUser = isUser
            };
            return View(viewModel);
        }

        public async Task<IActionResult> AdministrateUsers()
        {
            return View(await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> DetailUser(int? id)
        {
            if (id != null)
            {
                User user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult InsertUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertUser(User user)
        {
            User check = await _context.Users.FirstOrDefaultAsync(u => 
                u.Name == user.Name && u.Password == user.Password);
            if (check == null)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("AdministrateUsers");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(int? id)
        {
            if (id != null)
            {
                User user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("AdministrateUsers");
        }

        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id != null)
            {
                User user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AdministrateUsers");
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> ShowOperations()
        {
            return View(await _context.Operations.ToListAsync());
        }
    }
}
