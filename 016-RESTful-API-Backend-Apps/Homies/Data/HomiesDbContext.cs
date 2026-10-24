using Homies.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Homies.Data;

public class HomiesDbContext(DbContextOptions<HomiesDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventType> Types => Set<EventType>();
    public DbSet<EventParticipant> EventsParticipants => Set<EventParticipant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventParticipant>().HasKey(ep => new { ep.HelperId, ep.EventId });
        modelBuilder.Entity<Event>().Property(e => e.Name).HasMaxLength(20);
        modelBuilder.Entity<Event>().Property(e => e.Description).HasMaxLength(500);
        modelBuilder.Entity<EventType>().Property(t => t.Name).HasMaxLength(15);
        modelBuilder.Entity<EventType>().HasData(
            new EventType { Id = 1, Name = "Animals" },
            new EventType { Id = 2, Name = "Fun" },
            new EventType { Id = 3, Name = "Discussion" },
            new EventType { Id = 4, Name = "Work" });
    }
}
