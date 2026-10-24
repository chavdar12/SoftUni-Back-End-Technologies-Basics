using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MoviesLibraryAPI.Controllers;
using MoviesLibraryAPI.Data;
using MoviesLibraryAPI.Data.Models;
using MoviesLibraryAPI.Services;

namespace MoviesLibraryAPI.IntegrationTests;

public class MoviesLibraryControllerTests
{
    private SqliteConnection _connection = null!;
    private MoviesLibraryDbContext _context = null!;
    private MoviesLibraryController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _context = new MoviesLibraryDbContext(new DbContextOptionsBuilder<MoviesLibraryDbContext>().UseSqlite(_connection).Options);
        _context.Database.EnsureCreated();
        _controller = new MoviesLibraryController(new MoviesRepository(_context));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    private static Movie NewMovie(string title = "Test Movie") => new()
    {
        Title = title,
        Director = "Test Director",
        YearReleased = 2022,
        Genre = "Action",
        Duration = 120,
        Rating = 7.5,
    };

    [Test]
    public async Task AddAsync_ValidMovie_IsStored()
    {
        await _controller.AddAsync(NewMovie());

        var stored = await _controller.GetByTitleAsync("Test Movie");
        Assert.That(stored, Is.Not.Null);
        Assert.That(stored!.Director, Is.EqualTo("Test Director"));
    }

    [Test]
    public void AddAsync_InvalidMovie_Throws()
    {
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ValidationException>(() => _controller.AddAsync(null!));
            Assert.ThrowsAsync<ValidationException>(() => _controller.AddAsync(With(m => m.Title = "")));
            Assert.ThrowsAsync<ValidationException>(() => _controller.AddAsync(With(m => m.Director = new string('d', 101))));
            Assert.ThrowsAsync<ValidationException>(() => _controller.AddAsync(With(m => m.YearReleased = 1899)));
            Assert.ThrowsAsync<ValidationException>(() => _controller.AddAsync(With(m => m.Duration = 0)));
            Assert.ThrowsAsync<ValidationException>(() => _controller.AddAsync(With(m => m.Duration = 501)));
            Assert.ThrowsAsync<ValidationException>(() => _controller.AddAsync(With(m => m.Rating = 10.1)));
        });
        Assert.That(_context.Movies, Is.Empty);

        static Movie With(Action<Movie> change)
        {
            var movie = NewMovie();
            change(movie);
            return movie;
        }
    }

    [Test]
    public async Task AddAsync_DuplicateTitle_Throws()
    {
        await _controller.AddAsync(NewMovie());

        Assert.ThrowsAsync<InvalidOperationException>(() => _controller.AddAsync(NewMovie()));
    }

    [Test]
    public async Task DeleteAsync_ExistingTitle_RemovesMovie()
    {
        await _controller.AddAsync(NewMovie());

        await _controller.DeleteAsync("Test Movie");

        Assert.That(await _controller.GetByTitleAsync("Test Movie"), Is.Null);
    }

    [Test]
    public void DeleteAsync_MissingTitle_Throws()
    {
        Assert.ThrowsAsync<InvalidOperationException>(() => _controller.DeleteAsync("Missing"));
    }

    [TestCase("")]
    [TestCase("   ")]
    public void DeleteAsync_EmptyTitle_Throws(string title)
    {
        Assert.ThrowsAsync<ArgumentException>(() => _controller.DeleteAsync(title));
    }

    [Test]
    public async Task GetAllAsync_ReturnsMoviesSortedByTitle()
    {
        await _controller.AddAsync(NewMovie("Zulu"));
        await _controller.AddAsync(NewMovie("Alpha"));

        var movies = await _controller.GetAllAsync();

        Assert.That(movies.Select(m => m.Title), Is.EqualTo(new[] { "Alpha", "Zulu" }));
    }

    [Test]
    public async Task GetAllAsync_EmptyLibrary_ReturnsEmpty()
    {
        Assert.That(await _controller.GetAllAsync(), Is.Empty);
    }

    [Test]
    public async Task GetByTitleAsync_MissingTitle_ReturnsNull()
    {
        Assert.That(await _controller.GetByTitleAsync("Missing"), Is.Null);
    }

    [Test]
    public async Task SearchByTitleFragmentAsync_FindsMatchesCaseInsensitively()
    {
        await _controller.AddAsync(NewMovie("The Dark Knight"));
        await _controller.AddAsync(NewMovie("Dark City"));
        await _controller.AddAsync(NewMovie("Up"));

        var movies = await _controller.SearchByTitleFragmentAsync("DARK");

        Assert.That(movies.Select(m => m.Title), Is.EqualTo(new[] { "Dark City", "The Dark Knight" }));
    }

    [Test]
    public void SearchByTitleFragmentAsync_NoMatchOrEmpty_Throws()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.SearchByTitleFragmentAsync("nothing"));
        Assert.ThrowsAsync<ArgumentException>(() => _controller.SearchByTitleFragmentAsync(""));
    }

    [Test]
    public async Task UpdateAsync_ExistingMovie_ChangesFields()
    {
        await _controller.AddAsync(NewMovie());
        var updated = NewMovie();
        updated.Rating = 9.1;
        updated.Genre = "Drama";

        await _controller.UpdateAsync(updated);

        var stored = await _controller.GetByTitleAsync("Test Movie");
        Assert.That(stored!.Rating, Is.EqualTo(9.1));
        Assert.That(stored.Genre, Is.EqualTo("Drama"));
    }

    [Test]
    public async Task UpdateAsync_InvalidMovie_Throws()
    {
        await _controller.AddAsync(NewMovie());
        var invalid = NewMovie();
        invalid.Rating = -1;

        Assert.ThrowsAsync<ValidationException>(() => _controller.UpdateAsync(invalid));
    }

    [Test]
    public void UpdateAsync_MissingMovie_Throws()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.UpdateAsync(NewMovie()));
    }
}
