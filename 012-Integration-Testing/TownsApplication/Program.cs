using Microsoft.EntityFrameworkCore;
using TownsApplication.ControllerLayer;
using TownsApplication.Data;

var options = new DbContextOptionsBuilder<TownDbContext>().UseSqlite("Data Source=towns.db").Options;
using var context = new TownDbContext(options);
context.Database.EnsureCreated();

var controller = new TownController(context);
controller.AddTown("Sofia", 1_286_383);
controller.AddTown("Plovdiv", 346_893);

foreach (var town in controller.ListTowns())
{
    Console.WriteLine($"ID: {town.Id}, Name: {town.Name}, Population: {town.Population}");
}
