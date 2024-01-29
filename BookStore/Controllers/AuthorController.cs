using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookStore.Controllers
{
    // [Route("[controller]")]
    public class AuthorController : Controller
    {

        
        private readonly IBookstoreRepository<Author> authorRepository;

        public AuthorController(IBookstoreRepository<Author> authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        // Get: Author
        public ActionResult Index()
        {
            var authors = authorRepository.List();
            return View(authors);
        }

        // [HttpGet("{id}")]
        // Get: Autor/Details/5
        public ActionResult Details(int id)
        {
            var author = authorRepository.Find(id);
            return View(author);
        }

        
        // Get: Autor/Create
        // [HttpGet]
        public ActionResult Create ()
        {
            return View();
        }

        // Post: Autor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Author author)
        {
          try
          {
            authorRepository.Add(author);

            return RedirectToAction(nameof(Index));
          }
          catch
          {
            return View();
          }
        }

        // Get: Autor/Edit/5
        public ActionResult Edit(int id)
        {
          var author = authorRepository.Find(id);
          return View(author);
        }


        // Post: Autor/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Author author)
        {
          try
          {
            authorRepository.Update(id, author);

            return RedirectToAction(nameof(Index));
          }
          catch
          {
            return View();
          }
        }


        // Get: Autor/Delete/5
        public ActionResult Delete(int id)
        {
            var author = authorRepository.Find(id);
            return View(author);
        }

        // Post: Author/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Author author)
        {
          try
          {
            authorRepository.Delete(id);

            return RedirectToAction(nameof(Index));
          }
          catch
          {
            return View();
          }
        }

    }
}