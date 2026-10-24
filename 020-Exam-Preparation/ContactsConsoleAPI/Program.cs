using ContactsConsoleAPI.Business;
using ContactsConsoleAPI.Data.Models;
using ContactsConsoleAPI.DataAccess;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<ContactsDbContext>().UseSqlite("Data Source=contacts.db").Options;
await using var context = new ContactsDbContext(options);
await context.Database.EnsureCreatedAsync();

var manager = new ContactManager(new ContactRepository(context));
if (!context.Contacts.Any())
{
    await manager.AddAsync(new Contact
    {
        FirstName = "Ivan", LastName = "Petrov", Address = "1 Vitosha Blvd, Sofia", Phone = "+359888123456",
        Email = "ivan.petrov@example.com", Gender = "Male", ULID = "01HV8Z5Q3K4N7M2P",
    });
}

foreach (var contact in await manager.GetAllAsync())
{
    Console.WriteLine($"{contact.FirstName} {contact.LastName} <{contact.Email}> {contact.Phone}");
}
