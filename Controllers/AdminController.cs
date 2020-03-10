using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndyBooks.Models;
using IndyBooks.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IndyBooks.Controllers
{
    public class AdminController : Controller
    {
        private IndyBooksDataContext _db;
        public AdminController(IndyBooksDataContext db) { _db = db; }

        /***
         * CREATE
         */
        [HttpGet]
        public IActionResult CreateBook()
        {
            //TODO: Populate a new AddBookViewModel object with a complete set of Writers
            //      and send it on to the View "AddBook"
            var authorSelectOptions = _db.Writers.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name,
                
            }).ToList();
                
            AddBookViewModel addBookViewModel = new AddBookViewModel()
            {
                Writers = authorSelectOptions
            };

            return View("AddBook", addBookViewModel);
        }

        // Admin/CreateBook/
        [HttpPost]
        public IActionResult CreateBook(AddBookViewModel bookVM)
        {
            //TODO: Build the Writer object using the parameter
            Writer author = _db.Writers.SingleOrDefault(w => w.Id == bookVM.AuthorId);

            if (author == null)
            {
                author = new Writer
                {
                    Name = bookVM.Name
                };
            }

            // check if book exist
            Book book = _db.Books.SingleOrDefault(b => b.Id == bookVM.Id);

            // add if book doesn't exist
            if (book == null)
            {
                // 
                book = new Book
                {
                    Author = author,
                    Price = bookVM.Price,
                    SKU = bookVM.SKU,
                    Title = bookVM.Title
                };

                _db.Books.Add(book);
            }
            else
            {
                book.Author = author;
                book.Price = bookVM.Price;
                book.SKU = bookVM.SKU;
                book.Title = bookVM.Title;
     
                _db.Books.Update(book);
            }
            _db.SaveChanges();

            //Shows the book using the Index View 
            return RedirectToAction("Index", new { id = book.Id });
        }

        /***
         * READ       
         */
        [HttpGet]
        public IActionResult Index(long id)
        {
            // get all books from db

            var books = _db.Books
                .Include(b => b.Author)
                .ToList();
            // map book models to search result viewmodels

            var searchViewModel = books.Select(b => new SearchResultViewModel
            {
                BookId = b.Id,
                AuthorName = b.Author?.Name,
                Price = b.Price,
                SKU = b.SKU,
                Title = b.Title
            })
            .OrderBy(b => b.SKU)
            .ToList();
                

            //TODO: filter books by the id (if passed an id as its Route Parameter),
            //     otherwise use the entire collection of Books, ordered by SKU.
            if(id == 0)
            {
                return View("SearchResults", searchViewModel);
            }
            else
            {
                var booksById = searchViewModel
                    .Where(b => b.BookId == id);

                return View("SearchResults", booksById);
            }         
        }
        /***
         * UPDATE
         */
         //TODO: Write a method to take a book id, and load book and author info
         //      into the ViewModel for the AddBook View
         [HttpGet]
         public IActionResult UpdateBook(long id)
         {
            Book book = _db.Books
                .Include(b => b.Author)
                .FirstOrDefault(b => b.Id == id);
                
            var authorSelectOptions = _db.Writers.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name, 
                Selected = book.Author != null && book.Author.Id == a.Id
            }).ToList();

            AddBookViewModel addBookViewModel = new AddBookViewModel
            {
                Title = book.Title,
                Price = book.Price,
                SKU = book.SKU,
                Name = book.Author?.Name,
                Writers = authorSelectOptions,
                Id = book.Id,
                AuthorId = book.Author?.Id
            };

            return View("AddBook", addBookViewModel);
         }
        /***
         * DELETE
         */
        [HttpGet]
        public IActionResult DeleteBook(long id)
        {
            //TODO: Remove the Book associated with the given id number; Save Changes
            var book = _db.Books.Find(id);
            
            if(book != null)
            {
                _db.Books.Remove(book);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search() { return View(); }
        [HttpPost]
        public IActionResult Search(SearchViewModel search)
        {
            //Full Collection Search
            IQueryable<Book> foundBooks = _db.Books; // start with entire collection

            //Partial Title Search
            if (search.Title != null)
            {
                foundBooks = foundBooks
                            .Where(b => b.Title.Contains(search.Title))
                            .OrderBy(b => b.Author.Name)
                            ;
            }

            //Author's Last Name Search
            if (search.AuthorName != null)
            {
                //Use the Name property of the Book's Author entity
                foundBooks = foundBooks
                            .Include(b => b.Author)
                            .Where(b => b.Author.Name.Contains(search.AuthorName, StringComparison.CurrentCulture))
                            ;
            }
            //Priced Between Search (min and max price entered)
            if (search.MinPrice > 0 && search.MaxPrice > 0)
            {
                foundBooks = foundBooks
                            .Where(b => b.Price >= search.MinPrice && b.Price <= search.MaxPrice)
                            .OrderByDescending(b=>b.Price)
                            ;
            }
            //Highest Priced Book Search (only max price entered)
            if (search.MinPrice == 0 && search.MaxPrice > 0)
            {
                decimal max = _db.Books.Max(b => b.Price);
                foundBooks = foundBooks
                            .Where(b => b.Price == max)
                            ;
            }
            //Composite Search Results
            return View("SearchResults", foundBooks);
        }
    }
}