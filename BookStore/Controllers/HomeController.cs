using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookStore.Controllers
{
    [Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly BookStoreContext _context;

        public HomeController(BookStoreContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookStoreContext = _context.Books.Include(b => b.BookAuthor).Include(b => b.Store).Include(a => a.BookPictures);
            return View(await bookStoreContext.ToListAsync());
        }
        [HttpGet]
        public IActionResult Details(Guid? id)
        {
            Cart cart = new Cart()
            {
                Book = _context.Books.Include(b => b.BookAuthor).FirstOrDefault(m => m.Id.Equals(id)),
                Count = 1,
                BookId = (Guid)id,

            };
            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(Cart cart)
        {
            if (!ModelState.IsValid)
            {
                var cartItem = _context.Cart.FirstOrDefault(m => m.Id.Equals(cart.BookId));

                if(cartItem == null)
                {
                    _context.Cart.Add(cart);
                    _context.SaveChanges();
                }  
            }
            return RedirectToAction("Index");
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