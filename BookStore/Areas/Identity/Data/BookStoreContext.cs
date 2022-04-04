using BookStore.Areas.Identity.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

public class BookStoreContext : IdentityDbContext<BookStoreUser>
{
    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }
    public DbSet<Book> Books { get; set; }

    public DbSet<BookAuthor> BookAuthors { get; set; }

    public DbSet<Store> Stores { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<BookPictures> BookPictures { get; set; }

    public DbSet<Cart> Cart { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
