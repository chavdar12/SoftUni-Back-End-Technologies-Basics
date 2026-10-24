using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibroConsoleAPI.DataAccess;

public class LibroDbContext(DbContextOptions<LibroDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<Book>().HasIndex(b => b.ISBN).IsUnique();
}
