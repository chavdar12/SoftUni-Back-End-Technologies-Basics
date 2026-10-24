using System.ComponentModel.DataAnnotations;
using MoviesLibraryAPI.Data.Models;
using MoviesLibraryAPI.Services;

namespace MoviesLibraryAPI.Controllers;

public class MoviesLibraryController(IMoviesRepository repository)
{
    public async Task AddAsync(Movie movie)
    {
        Validate(movie);
        if (await repository.GetMovieByTitleAsync(movie.Title) != null)
        {
            throw new InvalidOperationException($"Movie with title '{movie.Title}' already exists.");
        }
        await repository.AddMovieAsync(movie);
    }

    public async Task DeleteAsync(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty.");
        }
        // The course skeleton forgot to await this call, so failures were silently lost.
        if (!await repository.DeleteMovieAsync(title))
        {
            throw new InvalidOperationException($"Movie with title '{title}' not found.");
        }
    }

    public async Task<IEnumerable<Movie>> GetAllAsync() => await repository.GetAllMoviesAsync();

    public Task<Movie?> GetByTitleAsync(string title) => repository.GetMovieByTitleAsync(title);

    public async Task<IEnumerable<Movie>> SearchByTitleFragmentAsync(string titleFragment)
    {
        if (string.IsNullOrWhiteSpace(titleFragment))
        {
            throw new ArgumentException("Title fragment cannot be empty.");
        }
        var movies = await repository.SearchMoviesByTitleAsync(titleFragment);
        return movies.Count > 0 ? movies : throw new KeyNotFoundException("No movies found.");
    }

    public async Task UpdateAsync(Movie movie)
    {
        Validate(movie);
        if (!await repository.UpdateMovieAsync(movie))
        {
            throw new KeyNotFoundException($"Movie with title '{movie.Title}' not found.");
        }
    }

    private static void Validate(Movie? movie)
    {
        if (movie == null)
        {
            throw new ValidationException("Movie is not valid.");
        }
        Validator.ValidateObject(movie, new ValidationContext(movie), validateAllProperties: true);
    }
}
