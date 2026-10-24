using System.ComponentModel.DataAnnotations;
using ContactsConsoleAPI.Business;
using ContactsConsoleAPI.Data.Models;
using ContactsConsoleAPI.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ContactsConsoleAPI.IntegrationTests;

public class ContactManagerIntegrationTests
{
    private SqliteConnection _connection = null!;
    private ContactsDbContext _context = null!;
    private ContactManager _manager = null!;

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _context = new ContactsDbContext(new DbContextOptionsBuilder<ContactsDbContext>().UseSqlite(_connection).Options);
        _context.Database.EnsureCreated();
        _manager = new ContactManager(new ContactRepository(_context));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    private static Contact NewContact(string ulid = "1ABC23456DEF", string firstName = "John", string lastName = "Doe") => new()
    {
        FirstName = firstName,
        LastName = lastName,
        Address = "123 Main St",
        Phone = "+359888123456",
        Email = "john.doe@example.com",
        Gender = "Male",
        ULID = ulid,
    };

    [Test]
    public async Task AddAsync_ValidContact_IsStored()
    {
        await _manager.AddAsync(NewContact());

        var stored = await _context.Contacts.SingleAsync();
        Assert.That(stored.ULID, Is.EqualTo("1ABC23456DEF"));
        Assert.That(stored.Email, Is.EqualTo("john.doe@example.com"));
    }

    [Test]
    public void AddAsync_InvalidContact_Throws()
    {
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(null!));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(With(c => c.Email = "not-an-email")));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(With(c => c.ULID = "short")));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(With(c => c.ULID = "lowercase12345")));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(With(c => c.Phone = "1234567890123456")));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(With(c => c.Phone = "call-me")));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(With(c => c.FirstName = "")));
            Assert.ThrowsAsync<ValidationException>(() => _manager.AddAsync(With(c => c.Gender = "Unspecified!")));
        });
        Assert.That(_context.Contacts, Is.Empty);

        static Contact With(Action<Contact> change)
        {
            var contact = NewContact();
            change(contact);
            return contact;
        }
    }

    [Test]
    public async Task AddAsync_DuplicateUlid_Throws()
    {
        await _manager.AddAsync(NewContact());

        Assert.ThrowsAsync<InvalidOperationException>(() => _manager.AddAsync(NewContact(firstName: "Jane")));
    }

    [Test]
    public async Task GetAllAsync_ReturnsContactsSortedByLastName()
    {
        await _manager.AddAsync(NewContact("AAAAAAAAAA1", lastName: "Zhivkov"));
        await _manager.AddAsync(NewContact("AAAAAAAAAA2", lastName: "Angelov"));

        var contacts = await _manager.GetAllAsync();

        Assert.That(contacts.Select(c => c.LastName), Is.EqualTo(new[] { "Angelov", "Zhivkov" }));
    }

    [Test]
    public void GetAllAsync_NoContacts_Throws() => Assert.ThrowsAsync<KeyNotFoundException>(() => _manager.GetAllAsync());

    [Test]
    public async Task SearchByFirstNameAsync_MatchesCaseInsensitively()
    {
        await _manager.AddAsync(NewContact("AAAAAAAAAA1", firstName: "Maria"));
        await _manager.AddAsync(NewContact("AAAAAAAAAA2", firstName: "Georgi"));

        var contacts = await _manager.SearchByFirstNameAsync("maria");

        Assert.That(contacts.Single().ULID, Is.EqualTo("AAAAAAAAAA1"));
    }

    [Test]
    public async Task SearchByFirstNameAsync_NoMatchOrEmpty_Throws()
    {
        await _manager.AddAsync(NewContact());

        Assert.ThrowsAsync<KeyNotFoundException>(() => _manager.SearchByFirstNameAsync("Nobody"));
        Assert.ThrowsAsync<ArgumentException>(() => _manager.SearchByFirstNameAsync(" "));
    }

    [Test]
    public async Task SearchByLastNameAsync_ReturnsAllWithThatLastName()
    {
        await _manager.AddAsync(NewContact("AAAAAAAAAA1", "Anna", "Ivanova"));
        await _manager.AddAsync(NewContact("AAAAAAAAAA2", "Elena", "Ivanova"));
        await _manager.AddAsync(NewContact("AAAAAAAAAA3", "Petar", "Petrov"));

        var contacts = await _manager.SearchByLastNameAsync("Ivanova");

        Assert.That(contacts.Select(c => c.FirstName), Is.EquivalentTo(new[] { "Anna", "Elena" }));
    }

    [Test]
    public void SearchByLastNameAsync_NoMatchOrEmpty_Throws()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(() => _manager.SearchByLastNameAsync("Nobody"));
        Assert.ThrowsAsync<ArgumentException>(() => _manager.SearchByLastNameAsync(""));
    }

    [Test]
    public async Task GetSpecificAsync_ExistingUlid_ReturnsContact()
    {
        await _manager.AddAsync(NewContact());

        Assert.That((await _manager.GetSpecificAsync("1ABC23456DEF")).FirstName, Is.EqualTo("John"));
    }

    [Test]
    public void GetSpecificAsync_MissingOrEmptyUlid_Throws()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(() => _manager.GetSpecificAsync("ZZZZZZZZZZZZ"));
        Assert.ThrowsAsync<ArgumentException>(() => _manager.GetSpecificAsync(""));
    }

    [Test]
    public async Task UpdateAsync_ExistingContact_ChangesFields()
    {
        await _manager.AddAsync(NewContact());
        var updated = NewContact();
        updated.Address = "42 New Street";
        updated.Email = "new@example.com";

        await _manager.UpdateAsync(updated);

        var stored = await _manager.GetSpecificAsync("1ABC23456DEF");
        Assert.That(stored.Address, Is.EqualTo("42 New Street"));
        Assert.That(stored.Email, Is.EqualTo("new@example.com"));
    }

    [Test]
    public async Task UpdateAsync_InvalidContact_ThrowsAndKeepsOriginal()
    {
        await _manager.AddAsync(NewContact());
        var invalid = NewContact();
        invalid.Email = "broken";

        Assert.ThrowsAsync<ValidationException>(() => _manager.UpdateAsync(invalid));
        Assert.That((await _manager.GetSpecificAsync("1ABC23456DEF")).Email, Is.EqualTo("john.doe@example.com"));
    }

    [Test]
    public void UpdateAsync_MissingContact_Throws() => Assert.ThrowsAsync<KeyNotFoundException>(() => _manager.UpdateAsync(NewContact()));

    [Test]
    public async Task DeleteAsync_ExistingContact_RemovesIt()
    {
        await _manager.AddAsync(NewContact());

        await _manager.DeleteAsync("1ABC23456DEF");

        Assert.That(_context.Contacts, Is.Empty);
    }

    [Test]
    public void DeleteAsync_MissingOrEmptyUlid_Throws()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(() => _manager.DeleteAsync("ZZZZZZZZZZZZ"));
        Assert.ThrowsAsync<ArgumentException>(() => _manager.DeleteAsync(" "));
    }
}
