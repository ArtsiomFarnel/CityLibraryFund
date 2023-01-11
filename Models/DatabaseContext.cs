using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CityLibraryFund.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.Role> Roles { get; set; }
        public DbSet<Entities.Book> Books { get; set; }
        public DbSet<Entities.Operation> Operations { get; set; }
        public DbSet<Entities.Library> Libraries { get; set; }
        public DbSet<Entities.Debt> Debts { get; set; }
        public DbSet<Entities.Copy> Copies { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            //Database.EnsureDeleted(); //delete a DB
            Database.EnsureCreated(); //create a DB
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //configure
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //initialize data
            //add roles
            modelBuilder.Entity<Entities.Role>().HasData(
                new Entities.Role[]
                {
                    new Entities.Role { Id = 1, Name = "admin" },
                    new Entities.Role { Id = 2, Name = "client" }
                });
            //add admin
            modelBuilder.Entity<Entities.User>().HasData(
                new Entities.User
                {
                    Id = 1,
                    Name = "admin",
                    RoleId = 1,
                    Password = "admin",
                    Email = "admin",
                    DateOfRegistration = DateTime.Now
                });
            //add books
            modelBuilder.Entity<Entities.Book>().HasData(
                new Entities.Book[]
                {
                    new Entities.Book 
                    {
                        Id = 1,
                        Name = "Хоббит",
                        Description = "Перед вами - самая любимая волшебная сказка для детей в самом любимом оформлении, знакомом каждому.",
                        Rating = 0,
                        Image = "/img/books/1.jpg",
                        IsNew = true,
                        Year = 1937,
                        Author = "Дж. Р. Р. Толкин",
                        Genre = "фэнтези,драма,комедия"
                    },
                    new Entities.Book 
                    {
                        Id = 2,
                        Name = "Властелин колец",
                        Description = "Трилогия Властелин Колец бесспорно возглавляет список культовых книг ХХ века.",
                        Rating = 0,
                        Image = "/img/books/2.jpg",
                        IsNew = true,
                        Year = 1954,
                        Author = "Дж. Р. Р. Толкин",
                        Genre = "фэнтези,боевик,драма"
                    },
                    new Entities.Book 
                    {
                        Id = 3,
                        Name = "Сильмариллион",
                        Description = "Сильмариллион - один из масштабнейших миров в истории фэнтези, мифологический канон, который Джон Руэл Толкин составлял на протяжении всей жизни.",
                        Rating = 0,
                        Image = "/img/books/3.jpg",
                        IsNew = true,
                        Year = 1977,
                        Author = "Дж. Р. Р. Толкин",
                        Genre = "фэнтези,драма,трилер"
                    },
                    new Entities.Book 
                    { 
                        Id = 4, 
                        Name = "Ведьмак", 
                        Description = "Сага о ведьмаке польского писателя Анджея Сапковского написана в жанре темного фэнтези.", 
                        Rating = 0,
                        Image = "/img/books/4.jpg",
                        IsNew = true,
                        Year = 1986,
                        Author = "А. Сапковский",
                        Genre = "фэнтези,боевик"
                    }
                });
            //add libraries
            modelBuilder.Entity<Entities.Library>().HasData(
                new Entities.Library[] 
                {
                    new Entities.Library
                    {
                        Id = 1,
                        Name = "Библиотека-филиал №1 им. Я. Коласа",
                        Description = "",
                        Address = "ул. Молодежная 104"                        
                    },
                    new Entities.Library
                    {
                        Id = 2,
                        Name = "Библиотека им. Маяковского",
                        Description = "",
                        Address = "ул. Калинина 1"
                    }
                });
            //add copies
            modelBuilder.Entity<Entities.Copy>().HasData(
                new Entities.Copy[]
                {
                    new Entities.Copy
                    {
                        Id = 1,
                        BookId = 1,
                        LibraryId = 1,
                        Amount = 5,
                        ProductDate = DateTime.Now
                    },
                    new Entities.Copy
                    {
                        Id = 2,
                        BookId = 2,
                        LibraryId = 2,
                        Amount = 4,
                        ProductDate = DateTime.Now
                    },
                    new Entities.Copy
                    {
                        Id = 3,
                        BookId = 3,
                        LibraryId = 1,
                        Amount = 3,
                        ProductDate = DateTime.Now
                    },
                    new Entities.Copy
                    {
                        Id = 4,
                        BookId = 4,
                        LibraryId = 2,
                        Amount = 2,
                        ProductDate = DateTime.Now
                    },
                    new Entities.Copy
                    {
                        Id = 5,
                        BookId = 2,
                        LibraryId = 1,
                        Amount = 2,
                        ProductDate = DateTime.Now
                    }
                });
            //add debts
            /*modelBuilder.Entity<Entities.Debt>().HasData(
                new Entities.Debt[]
                {
                    new Entities.Debt
                    {
                        Id = 1,
                        BookId = 1,
                        UserId = 1,
                        LibraryId = 1,
                        FirstDate = DateTime.Now,
                        LastDate = DateTime.Now,
                        Status = true,
                        PersonalRating = 0
                    }
                });*/
        }
    }
}
