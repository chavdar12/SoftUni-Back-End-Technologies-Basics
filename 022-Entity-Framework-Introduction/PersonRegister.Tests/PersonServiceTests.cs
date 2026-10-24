using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PersonRegister.Data;

namespace PersonRegister.Tests;

public class PersonServiceTests
{
    private SqliteConnection _connection = null!;
    private PersonRegisterDbContext _context = null!;
    private PersonService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _context = new PersonRegisterDbContext(new DbContextOptionsBuilder<PersonRegisterDbContext>().UseSqlite(_connection).Options);
        _context.Database.EnsureCreated();
        _service = new PersonService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    private async Task SeedAsync()
    {
        await _service.AddPersonAsync("Ivan", "Petrov", 34, "Sofia");
        await _service.AddPersonAsync("Maria", "Georgieva", 28, "Plovdiv");
        await _service.AddPersonAsync("Georgi", "Ivanov", 40, "Sofia");
    }

    [Test]
    public async Task AddPersonAsync_CreatesTownOnlyOnce()
    {
        await SeedAsync();

        Assert.That(await _context.People.CountAsync(), Is.EqualTo(3));
        Assert.That(await _context.Towns.Select(t => t.Name).OrderBy(n => n).ToListAsync(), Is.EqualTo(new[] { "Plovdiv", "Sofia" }));
    }

    [Test]
    public void AddPersonAsync_InvalidData_ThrowsAndSavesNothing()
    {
        Assert.ThrowsAsync<ValidationException>(() => _service.AddPersonAsync("", "Petrov", 30, "Sofia"));
        Assert.ThrowsAsync<ValidationException>(() => _service.AddPersonAsync("Ivan", "Petrov", -1, "Sofia"));
        Assert.ThrowsAsync<ValidationException>(() => _service.AddPersonAsync("Ivan", "Petrov", 30, "Sofia", "not-an-email"));
        Assert.That(_context.People, Is.Empty);
    }

    [Test]
    public async Task GetAllAsync_SortsByLastNameAndIncludesTown()
    {
        await SeedAsync();

        var people = await _service.GetAllAsync();

        Assert.That(people.Select(p => p.LastName), Is.EqualTo(new[] { "Georgieva", "Ivanov", "Petrov" }));
        Assert.That(people[0].Town.Name, Is.EqualTo("Plovdiv"));
    }

    [Test]
    public async Task GetByTownAsync_ReturnsOnlyThatTown()
    {
        await SeedAsync();

        var people = await _service.GetByTownAsync("Sofia");

        Assert.That(people.Select(p => p.FirstName), Is.EqualTo(new[] { "Georgi", "Ivan" }));
        Assert.That(await _service.GetByTownAsync("Varna"), Is.Empty);
    }

    [Test]
    public async Task GetOlderThanAsync_IsExclusiveAndSortedByAgeDescending()
    {
        await SeedAsync();

        var people = await _service.GetOlderThanAsync(28);

        Assert.That(people.Select(p => p.Age), Is.EqualTo(new[] { 40, 34 }));
    }

    [Test]
    public async Task GetTownStatsAsync_GroupsPeoplePerTown()
    {
        await SeedAsync();

        var stats = await _service.GetTownStatsAsync();

        Assert.That(stats, Is.EqualTo(new[]
        {
            new TownStats("Sofia", 2, 37),
            new TownStats("Plovdiv", 1, 28),
        }));
    }

    [Test]
    public async Task UpdateEmailAsync_ExistingPerson_SavesEmail()
    {
        var person = await _service.AddPersonAsync("Ivan", "Petrov", 34, "Sofia");

        Assert.That(await _service.UpdateEmailAsync(person.Id, "ivan@example.com"), Is.True);
        Assert.That((await _context.People.AsNoTracking().SingleAsync()).Email, Is.EqualTo("ivan@example.com"));
    }

    [Test]
    public async Task UpdateEmailAsync_MissingPersonOrInvalidEmail()
    {
        var person = await _service.AddPersonAsync("Ivan", "Petrov", 34, "Sofia");

        Assert.That(await _service.UpdateEmailAsync(999, "x@example.com"), Is.False);
        Assert.ThrowsAsync<ValidationException>(() => _service.UpdateEmailAsync(person.Id, "broken"));
    }

    [Test]
    public async Task DeletePersonAsync_RemovesPersonButKeepsTown()
    {
        var person = await _service.AddPersonAsync("Ivan", "Petrov", 34, "Sofia");

        Assert.That(await _service.DeletePersonAsync(person.Id), Is.True);
        Assert.That(await _service.DeletePersonAsync(person.Id), Is.False);
        Assert.That(_context.People, Is.Empty);
        Assert.That(await _context.Towns.CountAsync(), Is.EqualTo(1));
    }
}
