using Homies.Data;
using Homies.Models;
using Homies.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Homies.Tests;

public class EventServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 5, 1, 12, 0, 0, TimeSpan.Zero);

    private SqliteConnection _connection = null!;
    private HomiesDbContext _context = null!;
    private EventService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _context = new HomiesDbContext(new DbContextOptionsBuilder<HomiesDbContext>().UseSqlite(_connection).Options);
        _context.Database.EnsureCreated();
        _service = new EventService(_context, new FixedTimeProvider(Now));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    private static EventFormModel Form(string name = "Board games", int typeId = 2, int startDay = 10) => new()
    {
        Name = name,
        Description = "Bring your favourite game",
        Start = new DateTime(2026, 6, startDay, 18, 0, 0),
        End = new DateTime(2026, 6, startDay, 22, 0, 0),
        TypeId = typeId,
    };

    [Test]
    public async Task AddEventAsync_StoresEventWithOrganiserAndCreatedOn()
    {
        var id = await _service.AddEventAsync(Form(), "user-1");

        var stored = await _context.Events.SingleAsync();
        Assert.That(stored.Id, Is.EqualTo(id));
        Assert.That(stored.OrganiserId, Is.EqualTo("user-1"));
        Assert.That(stored.CreatedOn, Is.EqualTo(Now.UtcDateTime));
    }

    [Test]
    public void AddEventAsync_UnknownType_Throws()
    {
        Assert.ThrowsAsync<ArgumentException>(() => _service.AddEventAsync(Form(typeId: 99), "user-1"));
        Assert.That(_context.Events, Is.Empty);
    }

    [Test]
    public async Task GetAllEventsAsync_ReturnsEventsOrderedByStartWithTypeName()
    {
        await _service.AddEventAsync(Form("Later event", startDay: 20), "u");
        await _service.AddEventAsync(Form("Early event", typeId: 1, startDay: 5), "u");

        var events = (await _service.GetAllEventsAsync()).ToList();

        Assert.That(events.Select(e => e.Name), Is.EqualTo(new[] { "Early event", "Later event" }));
        Assert.That(events[0].Type, Is.EqualTo("Animals"));
    }

    [Test]
    public async Task GetEventDetailsAsync_ReturnsDetailsWithParticipants()
    {
        var id = await _service.AddEventAsync(Form(), "organiser");
        await _service.JoinEventAsync(id, "helper-1");
        await _service.JoinEventAsync(id, "helper-2");

        var details = await _service.GetEventDetailsAsync(id);

        Assert.That(details, Is.Not.Null);
        Assert.That(details!.Type, Is.EqualTo("Fun"));
        Assert.That(details.ParticipantsCount, Is.EqualTo(2));
    }

    [Test]
    public async Task GetEventDetailsAsync_MissingEvent_ReturnsNull()
    {
        Assert.That(await _service.GetEventDetailsAsync(123), Is.Null);
    }

    [Test]
    public async Task GetEventOrganiserIdAsync_ReturnsOrganiserOrNull()
    {
        var id = await _service.AddEventAsync(Form(), "organiser");

        Assert.That(await _service.GetEventOrganiserIdAsync(id), Is.EqualTo("organiser"));
        Assert.That(await _service.GetEventOrganiserIdAsync(id + 1), Is.Null);
    }

    [Test]
    public async Task UpdateEventAsync_ByOrganiser_UpdatesEvent()
    {
        var id = await _service.AddEventAsync(Form(), "organiser");

        var updated = await _service.UpdateEventAsync(id, Form("Chess night", typeId: 3), "organiser");

        Assert.That(updated, Is.True);
        var stored = await _context.Events.AsNoTracking().SingleAsync();
        Assert.That(stored.Name, Is.EqualTo("Chess night"));
        Assert.That(stored.TypeId, Is.EqualTo(3));
    }

    [Test]
    public async Task UpdateEventAsync_ByOtherUser_ReturnsFalseAndKeepsEvent()
    {
        var id = await _service.AddEventAsync(Form(), "organiser");

        var updated = await _service.UpdateEventAsync(id, Form("Hijacked"), "intruder");

        Assert.That(updated, Is.False);
        Assert.That((await _context.Events.AsNoTracking().SingleAsync()).Name, Is.EqualTo("Board games"));
    }

    [Test]
    public async Task UpdateEventAsync_MissingEvent_ReturnsFalse()
    {
        Assert.That(await _service.UpdateEventAsync(1, Form(), "organiser"), Is.False);
    }

    [Test]
    public async Task JoinEventAsync_AddsParticipantOnce()
    {
        var id = await _service.AddEventAsync(Form(), "organiser");

        Assert.That(await _service.JoinEventAsync(id, "helper"), Is.True);
        Assert.That(await _service.JoinEventAsync(id, "helper"), Is.False);
        Assert.That(await _context.EventsParticipants.CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task JoinEventAsync_MissingEvent_ReturnsFalse()
    {
        Assert.That(await _service.JoinEventAsync(42, "helper"), Is.False);
    }

    [Test]
    public async Task LeaveEventAsync_RemovesParticipant()
    {
        var id = await _service.AddEventAsync(Form(), "organiser");
        await _service.JoinEventAsync(id, "helper");

        Assert.That(await _service.LeaveEventAsync(id, "helper"), Is.True);
        Assert.That(await _service.LeaveEventAsync(id, "helper"), Is.False);
    }

    [Test]
    public async Task GetUserJoinedEventsAsync_ReturnsOnlyThatUsersEvents()
    {
        var first = await _service.AddEventAsync(Form("First event"), "organiser");
        var second = await _service.AddEventAsync(Form("Second event", startDay: 12), "organiser");
        await _service.JoinEventAsync(first, "helper");
        await _service.JoinEventAsync(second, "someone-else");

        var joined = (await _service.GetUserJoinedEventsAsync("helper")).ToList();

        Assert.That(joined, Has.Count.EqualTo(1));
        Assert.That(joined[0].Name, Is.EqualTo("First event"));
        Assert.That(joined[0].Type, Is.EqualTo("Fun"));
    }

    [Test]
    public async Task GetAllTypesAsync_ReturnsSeededTypes()
    {
        var types = await _service.GetAllTypesAsync();

        Assert.That(types.Select(t => t.Name), Is.EqualTo(new[] { "Animals", "Fun", "Discussion", "Work" }));
    }

    private sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now;
    }
}
