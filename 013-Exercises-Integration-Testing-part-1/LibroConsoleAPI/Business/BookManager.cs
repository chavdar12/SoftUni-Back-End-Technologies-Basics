using System.ComponentModel.DataAnnotations;
using LibroConsoleAPI.Data.Models;
using LibroConsoleAPI.DataAccess;

namespace LibroConsoleAPI.Business;

public class BookManager(IBookRepository repository) : IBookManager
{
    public async Task AddAsync(Book book)
    {
        Validate(book);
        if (await repository.GetBookByISBNAsync(book.ISBN) != null)
        {
            throw new InvalidOperationException($"A book with ISBN {book.ISBN} already exists.");
        }
        await repository.AddBookAsync(book);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        var books = await repository.GetAllBooksAsync();
        return books.Count > 0 ? books : throw new KeyNotFoundException("No books found.");
    }

    public async Task<IEnumerable<Book>> SearchByTitleAsync(string titleFragment)
    {
        RequireText(titleFragment, "Title fragment cannot be empty.");
        var books = await repository.SearchBooksByTitleAsync(titleFragment);
        return books.Count > 0 ? books : throw new KeyNotFoundException("No books found with the given title fragment.");
    }

    public async Task<Book> GetSpecificAsync(string isbn)
    {
        RequireText(isbn, "ISBN cannot be empty.");
        return await repository.GetBookByISBNAsync(isbn)
            ?? throw new KeyNotFoundException($"No book found with ISBN: {isbn}");
    }

    public async Task UpdateAsync(Book book)
    {
        Validate(book);
        if (!await repository.UpdateBookAsync(book))
        {
            throw new KeyNotFoundException($"No book found with ISBN: {book.ISBN}");
        }
    }

    public async Task DeleteAsync(string isbn)
    {
        RequireText(isbn, "ISBN cannot be empty.");
        if (!await repository.DeleteBookAsync(isbn))
        {
            throw new KeyNotFoundException($"No book found with ISBN: {isbn}");
        }
    }

    private static void Validate(Book? book)
    {
        if (book == null)
        {
            throw new ValidationException("Book is invalid.");
        }
        Validator.ValidateObject(book, new ValidationContext(book), validateAllProperties: true);
    }

    private static void RequireText(string value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(message);
        }
    }
}
