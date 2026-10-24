using MoviesLibraryAPI.Data.Models;

namespace MoviesLibraryAPI.Services;

public interface IMoviesRepository
{
    Task AddMovieAsync(Movie movie);
    Task<List<Movie>> GetAllMoviesAsync();
    Task<Movie?> GetMovieByTitleAsync(string title);
    Task<List<Movie>> SearchMoviesByTitleAsync(string titleFragment);
    Task<bool> UpdateMovieAsync(Movie movie);
    Task<bool> DeleteMovieAsync(string title);
}
