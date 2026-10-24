namespace LibraryManagement;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public bool IsCheckedOut { get; set; }
}

public class Library
{
    private readonly List<Book> _books = [];

    public void AddBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);
        if (_books.Any(b => b.Id == book.Id))
        {
            throw new InvalidOperationException($"A book with id {book.Id} already exists.");
        }
        _books.Add(book);
    }

    public bool CheckOutBook(int bookId) => SetCheckedOut(bookId, checkedOut: true);

    public bool ReturnBook(int bookId) => SetCheckedOut(bookId, checkedOut: false);

    public IReadOnlyList<Book> GetAllBooks() => _books.AsReadOnly();

    private bool SetCheckedOut(int bookId, bool checkedOut)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId && b.IsCheckedOut != checkedOut);
        if (book == null)
        {
            return false;
        }
        book.IsCheckedOut = checkedOut;
        return true;
    }
}
