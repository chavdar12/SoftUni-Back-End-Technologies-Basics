using LibroConsoleAPI.Data.Models;

namespace LibroConsoleAPI.DataAccess;

public interface IBookRepository
{
    Task AddBookAsync(Book book);
    Task<List<Book>> GetAllBooksAsync();
    Task<Book?> GetBookByISBNAsync(string isbn);
    Task<List<Book>> SearchBooksByTitleAsync(string titleFragment);
    Task<bool> UpdateBookAsync(Book book);
    Task<bool> DeleteBookAsync(string isbn);
}
