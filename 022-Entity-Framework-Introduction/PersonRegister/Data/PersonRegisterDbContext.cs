using Microsoft.EntityFrameworkCore;

namespace PersonRegister.Data;

public class PersonRegisterDbContext(DbContextOptions<PersonRegisterDbContext> options) : DbContext(options)
{
    public DbSet<Person> People => Set<Person>();
    public DbSet<Town> Towns => Set<Town>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Town>().HasIndex(t => t.Name).IsUnique();
        modelBuilder.Entity<Person>()
            .HasOne(p => p.Town)
            .WithMany(t => t.People)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
