using Microsoft.EntityFrameworkCore;
using MoviesLibraryAPI.Controllers;
using MoviesLibraryAPI.Data;
using MoviesLibraryAPI.Data.Models;
using MoviesLibraryAPI.Services;

var options = new DbContextOptionsBuilder<MoviesLibraryDbContext>().UseSqlite("Data Source=movies.db").Options;
await using var context = new MoviesLibraryDbContext(options);
await context.Database.EnsureCreatedAsync();

var controller = new MoviesLibraryController(new MoviesRepository(context));
if (await controller.GetByTitleAsync("Inception") == null)
{
    await controller.AddAsync(new Movie
    {
        Title = "Inception", Director = "Christopher Nolan", YearReleased = 2010,
        Genre = "Sci-Fi", Duration = 148, Rating = 8.8,
    });
}

foreach (var movie in await controller.GetAllAsync())
{
    Console.WriteLine($"{movie.Title} ({movie.YearReleased}) by {movie.Director} - {movie.Rating}/10");
}
