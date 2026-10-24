using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PersonRegister.Data;

namespace PersonRegister;

public record TownStats(string Town, int PeopleCount, double AverageAge);

public class PersonService(PersonRegisterDbContext context)
{
    public async Task<Person> AddPersonAsync(string firstName, string lastName, int age, string townName, string? email = null)
    {
        var town = await context.Towns.FirstOrDefaultAsync(t => t.Name == townName) ?? new Town { Name = townName };
        var person = new Person { FirstName = firstName, LastName = lastName, Age = age, Email = email, Town = town };
        Validator.ValidateObject(person, new ValidationContext(person), validateAllProperties: true);

        context.People.Add(person);
        await context.SaveChangesAsync();
        return person;
    }

    public Task<List<Person>> GetAllAsync() =>
        context.People.AsNoTracking().Include(p => p.Town).OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToListAsync();

    public Task<List<Person>> GetByTownAsync(string townName) =>
        context.People.AsNoTracking().Where(p => p.Town.Name == townName).OrderBy(p => p.FirstName).ToListAsync();

    public Task<List<Person>> GetOlderThanAsync(int age) =>
        context.People.AsNoTracking().Where(p => p.Age > age).OrderByDescending(p => p.Age).ToListAsync();

    public Task<List<TownStats>> GetTownStatsAsync() =>
        context.Towns
            .Where(t => t.People.Any())
            .OrderByDescending(t => t.People.Count)
            .ThenBy(t => t.Name)
            .Select(t => new TownStats(t.Name, t.People.Count, t.People.Average(p => p.Age)))
            .ToListAsync();

    public async Task<bool> UpdateEmailAsync(int personId, string? email)
    {
        var person = await context.People.FindAsync(personId);
        if (person == null)
        {
            return false;
        }

        person.Email = email;
        Validator.ValidateObject(person, new ValidationContext(person), validateAllProperties: true);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePersonAsync(int personId) =>
        await context.People.Where(p => p.Id == personId).ExecuteDeleteAsync() > 0;
}
