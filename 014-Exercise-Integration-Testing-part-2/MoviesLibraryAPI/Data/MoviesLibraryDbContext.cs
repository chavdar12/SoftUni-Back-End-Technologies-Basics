using Microsoft.EntityFrameworkCore;
using MoviesLibraryAPI.Data.Models;

namespace MoviesLibraryAPI.Data;

public class MoviesLibraryDbContext(DbContextOptions<MoviesLibraryDbContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<Movie>().HasIndex(m => m.Title).IsUnique();
}
