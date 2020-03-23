using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Library.Controllers
{
  [Authorize]
  public class BooksController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager; //new line

    public BooksController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      //var userItems = _db.Items.Where(entry => entry.User.Id == currentUser.Id);
      List<Book> model = _db.Books.ToList();
      ViewBag.IsLibrarian = currentUser.IsLibrarian;
      return View(model);
    }

    public async Task<ActionResult> Create()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.IsLibrarian = currentUser.IsLibrarian;
      return View();
    }

    [HttpPost]
    public ActionResult Create(Book book)
    {
      _db.Books.Add(book);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> Details(int id)
    {
      var thisBook = _db.Books
        .Include(book => book.Authors)
        .ThenInclude(join => join.Author)
        .Include(book => book.CheckoutHistory)
        .ThenInclude(checkout => checkout.User)
        .FirstOrDefault(book => book.BookId == id);
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.IsLibrarian = currentUser.IsLibrarian;
      return View(thisBook);
    }

    public async Task<ActionResult> Edit(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.IsLibrarian = currentUser.IsLibrarian;
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult Edit(Book book)
    {
      _db.Entry(book).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> Delete(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.IsLibrarian = currentUser.IsLibrarian;
      return View(thisBook);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      _db.Books.Remove(thisBook);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> AddAuthor(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.IsLibrarian = currentUser.IsLibrarian;
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult AddAuthor(Book book, int AuthorId)
    {
      if (AuthorId != 0)
      {
        _db.BookAuthor.Add(new BookAuthor() { AuthorId = AuthorId, BookId = book.BookId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteAuthor(int joinId)
    {
      var joinEntry = _db.BookAuthor.FirstOrDefault(entry => entry.BookAuthorId == joinId);
      _db.BookAuthor.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<ActionResult> CreateCheckout(int bookId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var thisBook = _db.Books.Include(book => book.CheckoutHistory).FirstOrDefault(book => book.BookId == bookId);
      foreach(Checkout checkout in thisBook.CheckoutHistory)
      {
        checkout.Active = false;
        _db.Entry(checkout).State = EntityState.Modified;
      }
      _db.Entry(thisBook).Property(book => book.Out).CurrentValue = 1;
      _db.Entry(thisBook).State = EntityState.Modified;
      _db.Checkouts.Add( new Checkout() {
        DateIn = DateTime.Now,
        DateDue = DateTime.Now.Add(new System.TimeSpan(0, 0, 0, 10)), // 14, 0, 0, 0 = 2 weeks
        Book = thisBook,
        Active = true,
        User = currentUser
      });
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult ReturnCheckout(int bookId)
    {
      var thisBook = _db.Books.Include(book => book.CheckoutHistory).FirstOrDefault(book => book.BookId == bookId);
      foreach(Checkout checkout in thisBook.CheckoutHistory)
      {
        checkout.Active = false;
        _db.Entry(checkout).State = EntityState.Modified;
      }
      _db.Entry(thisBook).Property(book => book.Out).CurrentValue = 0;
      _db.Entry(thisBook).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
