using Microsoft.EntityFrameworkCore;
using MoviesLibraryAPI.Data;
using MoviesLibraryAPI.Data.Models;

namespace MoviesLibraryAPI.Services;

public class MoviesRepository(MoviesLibraryDbContext context) : IMoviesRepository
{
    public async Task AddMovieAsync(Movie movie)
    {
        context.Movies.Add(movie);
        await context.SaveChangesAsync();
    }

    public Task<List<Movie>> GetAllMoviesAsync() => context.Movies.AsNoTracking().OrderBy(m => m.Title).ToListAsync();

    public Task<Movie?> GetMovieByTitleAsync(string title) =>
        context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Title == title);

    public Task<List<Movie>> SearchMoviesByTitleAsync(string titleFragment) =>
        context.Movies.AsNoTracking()
            .Where(m => m.Title.ToLower().Contains(titleFragment.ToLower()))
            .OrderBy(m => m.Title)
            .ToListAsync();

    public async Task<bool> UpdateMovieAsync(Movie movie)
    {
        var existing = await context.Movies.FirstOrDefaultAsync(m => m.Title == movie.Title);
        if (existing == null)
        {
            return false;
        }

        existing.Director = movie.Director;
        existing.YearReleased = movie.YearReleased;
        existing.Genre = movie.Genre;
        existing.Duration = movie.Duration;
        existing.Rating = movie.Rating;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMovieAsync(string title) =>
        await context.Movies.Where(m => m.Title == title).ExecuteDeleteAsync() > 0;
}
