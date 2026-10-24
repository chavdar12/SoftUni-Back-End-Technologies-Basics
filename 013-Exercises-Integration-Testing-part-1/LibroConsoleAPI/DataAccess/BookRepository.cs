using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibroConsoleAPI.DataAccess;

public class BookRepository(LibroDbContext context) : IBookRepository
{
    public async Task AddBookAsync(Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
    }

    public Task<List<Book>> GetAllBooksAsync() => context.Books.AsNoTracking().OrderBy(b => b.Title).ToListAsync();

    public Task<Book?> GetBookByISBNAsync(string isbn) => context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.ISBN == isbn);

    public Task<List<Book>> SearchBooksByTitleAsync(string titleFragment) =>
        context.Books.AsNoTracking()
            .Where(b => b.Title.ToLower().Contains(titleFragment.ToLower()))
            .OrderBy(b => b.Title)
            .ToListAsync();

    public async Task<bool> UpdateBookAsync(Book book)
    {
        var existing = await context.Books.FirstOrDefaultAsync(b => b.ISBN == book.ISBN);
        if (existing == null)
        {
            return false;
        }

        existing.Title = book.Title;
        existing.Author = book.Author;
        existing.YearPublished = book.YearPublished;
        existing.Genre = book.Genre;
        existing.Pages = book.Pages;
        existing.Price = book.Price;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBookAsync(string isbn) =>
        await context.Books.Where(b => b.ISBN == isbn).ExecuteDeleteAsync() > 0;
}
