using LibroConsoleAPI.Business;
using LibroConsoleAPI.Data.Models;
using LibroConsoleAPI.DataAccess;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<LibroDbContext>().UseSqlite("Data Source=libro.db").Options;
await using var context = new LibroDbContext(options);
await context.Database.EnsureCreatedAsync();

var manager = new BookManager(new BookRepository(context));
if (!context.Books.Any())
{
    await manager.AddAsync(new Book
    {
        Title = "Under the Yoke", Author = "Ivan Vazov", ISBN = "9789540901234",
        YearPublished = 1894, Genre = "Historical novel", Pages = 400, Price = 19.90,
    });
}

foreach (var book in await manager.GetAllAsync())
{
    Console.WriteLine($"{book.Title} by {book.Author} ({book.YearPublished}) - ISBN {book.ISBN}, {book.Price:F2} lv.");
}
