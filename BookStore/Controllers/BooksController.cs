#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private readonly BookStoreContext _context;
        private IWebHostEnvironment webHostEnvironment;

        public BooksController(IWebHostEnvironment webHostEnvironment, BookStoreContext context)
        {
            this.webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var bookStoreContext = _context.Books.Include(b => b.BookAuthor).Include(b => b.Store);
            return View(await bookStoreContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(a=>a.BookPictures)
                .Include(b => b.BookAuthor)
                .Include(b => b.Store)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["BookAuthorId"] = new SelectList(_context.BookAuthors, "Id", "Name");
            ViewData["StoreId"] = new SelectList(_context.Stores, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var model = new Book()
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    ISBN = vm.ISBN,
                    Price = vm.Price,
                    BookAuthorId = vm.BookAuthorId,
                    StoreId = vm.StoreId,
                };
                foreach (var item in vm.Pictures)
                {
                    model.BookPictures.Add(new BookPictures()
                    {
                        PictureUri = UploadImage(item),
                        Book = model
                    });
                }
               _context.Books.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        private string UploadImage(IFormFile item)
        {
            string fileName = null;
            if (item !=null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + item.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    item.CopyTo(fileStream);
                }
                
            }
            return fileName;
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["BookAuthorId"] = new SelectList(_context.BookAuthors, "Id", "Id", book.BookAuthorId);
            ViewData["StoreId"] = new SelectList(_context.Stores, "Id", "Name", book.StoreId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, [Bind("Id,Name,Description,ISBN,Price,BookAuthorId,StoreId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookAuthorId"] = new SelectList(_context.BookAuthors, "Id", "Id", book.BookAuthorId);
            ViewData["StoreId"] = new SelectList(_context.Stores, "Id", "Name", book.StoreId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(a => a.BookPictures)
                .Include(b => b.BookAuthor)
                .Include(b => b.Store)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(Guid? id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
