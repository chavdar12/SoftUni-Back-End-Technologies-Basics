using TownsApplication.Data;
using TownsApplication.Data.Models;

namespace TownsApplication.ControllerLayer;

public class TownController(TownDbContext context)
{
    public void AddTown(string townName, int population)
    {
        if (string.IsNullOrWhiteSpace(townName) || townName.Length < 3)
        {
            throw new ArgumentException("Invalid town name.");
        }
        ValidatePopulation(population);

        if (GetTownByName(townName) != null)
        {
            return;
        }

        context.Towns.Add(new Town { Name = townName, Population = population });
        context.SaveChanges();
    }

    public void UpdateTown(int id, int newPopulation)
    {
        ValidatePopulation(newPopulation);

        var town = context.Towns.Find(id);
        if (town != null)
        {
            town.Population = newPopulation;
            context.SaveChanges();
        }
    }

    public void DeleteTown(int id)
    {
        var town = context.Towns.Find(id);
        if (town != null)
        {
            context.Towns.Remove(town);
            context.SaveChanges();
        }
    }

    public List<Town> ListTowns() => context.Towns.OrderBy(t => t.Id).ToList();

    public Town? GetTownByName(string townName) => context.Towns.FirstOrDefault(t => t.Name == townName);

    private static void ValidatePopulation(int population)
    {
        if (population <= 0)
        {
            throw new ArgumentException("Population must be a positive number.");
        }
    }
}
