using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TownsApplication.ControllerLayer;
using TownsApplication.Data;

namespace TownsApplication.IntegrationTests;

public class TownControllerIntegrationTests
{
    private SqliteConnection _connection = null!;
    private TownDbContext _context = null!;
    private TownController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        // An open in-memory SQLite connection is a real relational database that lives for one test.
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _context = new TownDbContext(new DbContextOptionsBuilder<TownDbContext>().UseSqlite(_connection).Options);
        _context.Database.EnsureCreated();
        _controller = new TownController(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Test]
    public void AddTown_ValidInput_PersistsTown()
    {
        _controller.AddTown("Sofia", 1_000_000);

        var town = _context.Towns.Single();
        Assert.That(town.Name, Is.EqualTo("Sofia"));
        Assert.That(town.Population, Is.EqualTo(1_000_000));
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase("AB")]
    public void AddTown_InvalidName_Throws(string name)
    {
        var ex = Assert.Throws<ArgumentException>(() => _controller.AddTown(name, 1000));
        Assert.That(ex.Message, Is.EqualTo("Invalid town name."));
        Assert.That(_context.Towns, Is.Empty);
    }

    [TestCase(0)]
    [TestCase(-5)]
    public void AddTown_NonPositivePopulation_Throws(int population)
    {
        var ex = Assert.Throws<ArgumentException>(() => _controller.AddTown("Sofia", population));
        Assert.That(ex.Message, Is.EqualTo("Population must be a positive number."));
    }

    [Test]
    public void AddTown_DuplicateName_IsIgnored()
    {
        _controller.AddTown("Sofia", 100);
        _controller.AddTown("Sofia", 200);

        Assert.That(_context.Towns.Count(), Is.EqualTo(1));
        Assert.That(_controller.GetTownByName("Sofia")!.Population, Is.EqualTo(100));
    }

    [Test]
    public void UpdateTown_ExistingTown_ChangesPopulation()
    {
        _controller.AddTown("Varna", 300_000);
        var id = _controller.GetTownByName("Varna")!.Id;

        _controller.UpdateTown(id, 350_000);

        Assert.That(_controller.GetTownByName("Varna")!.Population, Is.EqualTo(350_000));
    }

    [Test]
    public void UpdateTown_InvalidPopulation_Throws()
    {
        _controller.AddTown("Varna", 300_000);
        var id = _controller.GetTownByName("Varna")!.Id;

        Assert.Throws<ArgumentException>(() => _controller.UpdateTown(id, 0));
        Assert.That(_controller.GetTownByName("Varna")!.Population, Is.EqualTo(300_000));
    }

    [Test]
    public void UpdateTown_MissingTown_DoesNothing()
    {
        Assert.DoesNotThrow(() => _controller.UpdateTown(999, 10));
    }

    [Test]
    public void DeleteTown_ExistingTown_RemovesIt()
    {
        _controller.AddTown("Burgas", 200_000);
        var id = _controller.GetTownByName("Burgas")!.Id;

        _controller.DeleteTown(id);

        Assert.That(_controller.GetTownByName("Burgas"), Is.Null);
    }

    [Test]
    public void DeleteTown_MissingTown_DoesNothing()
    {
        _controller.AddTown("Burgas", 200_000);

        _controller.DeleteTown(999);

        Assert.That(_context.Towns.Count(), Is.EqualTo(1));
    }

    [Test]
    public void ListTowns_ReturnsAllTownsInInsertOrder()
    {
        _controller.AddTown("Sofia", 1);
        _controller.AddTown("Plovdiv", 2);
        _controller.AddTown("Varna", 3);

        Assert.That(_controller.ListTowns().Select(t => t.Name), Is.EqualTo(new[] { "Sofia", "Plovdiv", "Varna" }));
    }

    [Test]
    public void ListTowns_EmptyDatabase_ReturnsEmptyList()
    {
        Assert.That(_controller.ListTowns(), Is.Empty);
    }
}
