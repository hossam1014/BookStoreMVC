using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Repositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace BookStore.Controllers
{

    public class BookController : Controller
    {

        public readonly IBookstoreRepository<Book> bookRepository;
        public IBookstoreRepository<Author> authorRepository;
        public IWebHostEnvironment hosting; 

        public BookController(IBookstoreRepository<Book> bookRepository,
          IBookstoreRepository<Author> authorRepository,
          IWebHostEnvironment hosting)
        {
          this.hosting = hosting;

          this.authorRepository = authorRepository;

          this.bookRepository = bookRepository;

        }

        // GET: Book
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View(GetAllAuthors());
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
        if (ModelState.IsValid)
        {
          try
          {
            string fileName = string.Empty;
            if (model.File != null)
            {
              string uploads = Path.Combine(hosting.WebRootPath, "uploads");
              fileName = model.File.FileName;
              string fullPath = Path.Combine(uploads, fileName);
              model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
            }

            if(model.AuthorId == -1)
            {
              ViewBag.Message = "Pease select an author from the list!";
              
              return View(GetAllAuthors());
            }

            var author = authorRepository.Find(model.AuthorId);
            Book book = new Book
            {
              Id = model.BookId,
              Title = model.Title,
              Description = model.Description,
              Author = author,
              ImageUrl = fileName
            };
            bookRepository.Add(book);
            return RedirectToAction(nameof(Index));
          }
          catch
          {
          return View();
          }
        }
        
        ModelState.AddModelError("", "You have to fill all required fields!");
        return View(GetAllAuthors());

      }


        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {

            var book = bookRepository.Find(id);

            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;
            
            var viewModel = new BookAuthorViewModel
            {
              BookId = book.Id,
              Title = book.Title,
              Description = book.Description,
              AuthorId = authorId,
              Authors = authorRepository.List().ToList(),
              ImageUrl = book.ImageUrl
            };
            return View(viewModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookAuthorViewModel viewModel)
        {
          try
          {
            string fileName = string.Empty;
            if (viewModel.File != null)
            {
              string uploads = Path.Combine(hosting.WebRootPath, "uploads");
              fileName = viewModel.File.FileName;
              string fullPath = Path.Combine(uploads, fileName);
              
              // Delete the old file
              string oldFileName = bookRepository.Find(viewModel.BookId).ImageUrl;
              string fullOldPath = Path.Combine(uploads, oldFileName);

              if (fullPath != fullOldPath)
              {
                System.IO.File.Delete(fullOldPath);
                // Save the new file
                viewModel.File.CopyTo(new FileStream(fullPath, FileMode.Create));
              }
              
            }


            var author = authorRepository.Find(viewModel.AuthorId);
            Book book = new Book
            {
              Title = viewModel.Title,
              Description = viewModel.Description,
              Author = author,
              ImageUrl = fileName
            };
            bookRepository.Update(viewModel.BookId, book);
            return RedirectToAction(nameof(Index));
          }
          catch
          {
          return View();
          }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);
            
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                bookRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        List<Author> FillSelectList()
        {
          var authors = authorRepository.List().ToList();
          authors.Insert(0, new Author { Id=-1, FullName="--- Please select an author ---"});
          return authors;
        }


        BookAuthorViewModel GetAllAuthors()
        {
          var vmodel = new BookAuthorViewModel
            {
              Authors = FillSelectList()
            };
          return vmodel;
        }
    }
}