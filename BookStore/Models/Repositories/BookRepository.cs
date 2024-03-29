using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;

namespace BookStore.Models.Repositories
{
  public class BookRepository : IBookstoreRepository<Book>
  {
    List<Book> books;

    public BookRepository()
    {
      books = new List<Book>()
      {
        new Book
        {
          Id = 1, Title = "c# Programing", 
          Description = "Empty", 
          Author = new Author{Id =2},
          ImageUrl = "xz.png"
        },
        new Book
        {
          Id = 2, 
          Title = "c++ Programing", 
          Description = "Nothing", 
          Author = new Author(),
          ImageUrl = "AM.jpg"
        },
        new Book
        {
          Id = 3, 
          Title = "Java Programing", 
          Description = "No Data", 
          Author = new Author(),
          ImageUrl = "SAH.png"
        }
      };
    }

    public void Add(Book entity)
    {
      entity.Id = books.Max(b => b.Id) + 1;
      books.Add(entity);
    }

    public void Delete(int id)
    {
      var book = Find(id);

      books.Remove(book);
    }

    public Book Find(int id)
    {
      var book = books.SingleOrDefault(b => b.Id == id);
      return book;
    }

    public IList<Book> List()
    {
      return books;
    }

    public void Update(int id, Book newBook)
    {
      var book = Find(id);

      book.Title = newBook.Title;
      book.Description = newBook.Description;
      book.Author = newBook.Author;
      book.ImageUrl = newBook.ImageUrl;
    }
  }
}