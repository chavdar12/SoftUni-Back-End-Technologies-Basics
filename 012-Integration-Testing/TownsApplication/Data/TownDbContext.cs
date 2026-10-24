using Microsoft.EntityFrameworkCore;
using TownsApplication.Data.Models;

namespace TownsApplication.Data;

public class TownDbContext(DbContextOptions<TownDbContext> options) : DbContext(options)
{
    public DbSet<Town> Towns => Set<Town>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<Town>().HasIndex(t => t.Name).IsUnique();
}
