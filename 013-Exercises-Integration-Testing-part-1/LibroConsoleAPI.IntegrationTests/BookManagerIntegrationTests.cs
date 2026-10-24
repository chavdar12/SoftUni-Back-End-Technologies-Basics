using System.ComponentModel.DataAnnotations;
using LibroConsoleAPI.Business;
using LibroConsoleAPI.Data.Models;
using LibroConsoleAPI.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LibroConsoleAPI.IntegrationTests;

public class BookManagerIntegrationTests
{
    private SqliteConnection _connection = null!;
    private LibroDbContext _context = null!;
    private BookManager _manager = null!;

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _context = new LibroDbContext(new DbContextOptionsBuilder<LibroDbContext>().UseSqlite(_connection).Options);
        _context.Database.EnsureCreated();
        _manager = new BookManager(new BookRepository(_context));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    private static Book NewBook(string isbn = "1234567890123", string title = "Test Book") => new()
    {
        Title = title,
        Author = "John Doe",
        ISBN = isbn,
        YearPublished = 2021,
        Genre = "Fiction",
        Pages = 100,
        Price = 19.99,
    };

    [Test]
    public async Task AddAsync_ValidBook_IsStored()
    {
        await _manager.AddAsync(NewBook());

        var stored = await _context.Books.SingleAsync();
        Assert.That(stored.Title, Is.EqualTo("Test Book"));
        Assert.That(stored.ISBN, Is.EqualTo("1234567890123"));
    }

    [TestCase("123")]
    [TestCase("12345678901234")]
    [TestCase("12345678901ab")]
    public void AddAsync_InvalidIsbn_Throws(string isbn)
    {
        Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(NewBook(isbn)));
        Assert.That(_context.Books, Is.Empty);
    }

    [Test]
    public void AddAsync_InvalidFields_Throws()
    {
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(null!));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(Modify(b => b.Title = new string('a', 256))));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(Modify(b => b.Author = "")));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(Modify(b => b.Pages = 0)));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(Modify(b => b.Price = 0)));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(Modify(b => b.YearPublished = 1699)));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(Modify(b => b.YearPublished = DateTime.UtcNow.Year + 1)));
        });
        Assert.That(_context.Books, Is.Empty);

        static Book Modify(Action<Book> change)
        {
            var book = NewBook();
            change(book);
            return book;
        }
    }

    [Test]
    public async Task AddAsync_DuplicateIsbn_Throws()
    {
        await _manager.AddAsync(NewBook());

        Assert.ThrowsAsync<InvalidOperationException>(() => _manager.AddAsync(NewBook(title: "Other")));
    }

    [Test]
    public async Task GetAllAsync_ReturnsBooksSortedByTitle()
    {
        await _manager.AddAsync(NewBook("1111111111111", "Zebra"));
        await _manager.AddAsync(NewBook("2222222222222", "Apple"));

        var books = await _manager.GetAllAsync();

        Assert.That(books.Select(b => b.Title), Is.EqualTo(new[] { "Apple", "Zebra" }));
    }

    [Test]
    public void GetAllAsync_NoBooks_Throws()
    {
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _manager.GetAllAsync());
        Assert.That(ex!.Message, Is.EqualTo("No books found."));
    }

    [Test]
    public async Task SearchByTitleAsync_MatchesFragmentCaseInsensitively()
    {
        await _manager.AddAsync(NewBook("1111111111111", "The Great Gatsby"));
        await _manager.AddAsync(NewBook("2222222222222", "Great Expectations"));
        await _manager.AddAsync(NewBook("3333333333333", "Dune"));

        var books = await _manager.SearchByTitleAsync("great");

        Assert.That(books.Select(b => b.Title), Is.EquivalentTo(new[] { "The Great Gatsby", "Great Expectations" }));
    }

    [Test]
    public async Task SearchByTitleAsync_NoMatch_Throws()
    {
        await _manager.AddAsync(NewBook());

        Assert.ThrowsAsync<KeyNotFoundException>(() => _manager.SearchByTitleAsync("missing"));
    }

    [TestCase("")]
    [TestCase("  ")]
    public void SearchByTitleAsync_EmptyFragment_Throws(string fragment)
    {
        Assert.ThrowsAsync<ArgumentException>(() => _manager.SearchByTitleAsync(fragment));
    }

    [Test]
    public async Task GetSpecificAsync_ExistingIsbn_ReturnsBook()
    {
        await _manager.AddAsync(NewBook());

        var book = await _manager.GetSpecificAsync("1234567890123");

        Assert.That(book.Title, Is.EqualTo("Test Book"));
    }

    [Test]
    public void GetSpecificAsync_MissingOrEmptyIsbn_Throws()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(() => _manager.GetSpecificAsync("9999999999999"));
        Assert.ThrowsAsync<ArgumentException>(() => _manager.GetSpecificAsync(" "));
    }

    [Test]
    public async Task UpdateAsync_ExistingBook_ChangesFields()
    {
        await _manager.AddAsync(NewBook());
        var updated = NewBook();
        updated.Title = "Updated Title";
        updated.Price = 5.5;

        await _manager.UpdateAsync(updated);

        var stored = await _manager.GetSpecificAsync("1234567890123");
        Assert.That(stored.Title, Is.EqualTo("Updated Title"));
        Assert.That(stored.Price, Is.EqualTo(5.5));
    }

    [Test]
    public async Task UpdateAsync_InvalidBook_ThrowsAndKeepsOriginal()
    {
        await _manager.AddAsync(NewBook());
        var invalid = NewBook();
        invalid.Title = "";

        Assert.ThrowsAsync<ValidationException>(() => _manager.UpdateAsync(invalid));
        Assert.That((await _manager.GetSpecificAsync("1234567890123")).Title, Is.EqualTo("Test Book"));
    }

    [Test]
    public void UpdateAsync_MissingBook_Throws()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(() => _manager.UpdateAsync(NewBook()));
        Assert.That(_context.Books, Is.Empty);
    }

    [Test]
    public async Task DeleteAsync_ExistingBook_RemovesIt()
    {
        await _manager.AddAsync(NewBook());

        await _manager.DeleteAsync("1234567890123");

        Assert.That(_context.Books, Is.Empty);
    }

    [Test]
    public void DeleteAsync_MissingOrEmptyIsbn_Throws()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(() => _manager.DeleteAsync("9999999999999"));
        Assert.ThrowsAsync<ArgumentException>(() => _manager.DeleteAsync(""));
    }
}
