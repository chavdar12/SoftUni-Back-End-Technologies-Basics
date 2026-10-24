using ContactsConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsConsoleAPI.DataAccess;

public class ContactsDbContext(DbContextOptions<ContactsDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<Contact>().HasIndex(c => c.ULID).IsUnique();
}
