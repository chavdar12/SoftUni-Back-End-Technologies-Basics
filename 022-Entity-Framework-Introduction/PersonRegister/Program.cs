using Microsoft.EntityFrameworkCore;
using PersonRegister;
using PersonRegister.Data;

var options = new DbContextOptionsBuilder<PersonRegisterDbContext>().UseSqlite("Data Source=people.db").Options;
await using var context = new PersonRegisterDbContext(options);
await context.Database.EnsureCreatedAsync();

var service = new PersonService(context);
if (!context.People.Any())
{
    await service.AddPersonAsync("Ivan", "Petrov", 34, "Sofia", "ivan@example.com");
    await service.AddPersonAsync("Maria", "Georgieva", 28, "Plovdiv");
    await service.AddPersonAsync("Georgi", "Ivanov", 41, "Sofia");
}

foreach (var person in await service.GetAllAsync())
{
    Console.WriteLine($"{person.FirstName} {person.LastName}, {person.Age} - {person.Town.Name}");
}
foreach (var stats in await service.GetTownStatsAsync())
{
    Console.WriteLine($"{stats.Town}: {stats.PeopleCount} people, average age {stats.AverageAge:F1}");
}
