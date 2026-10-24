using Homies.Data;
using Homies.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Resolve the connection string lazily so tests can override it through configuration.
builder.Services.AddDbContext<HomiesDbContext>((services, options) =>
    options.UseSqlite(services.GetRequiredService<IConfiguration>().GetConnectionString("Homies")));
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<HomiesDbContext>().Database.EnsureCreated();
}

app.MapControllers();
app.Run();

public partial class Program;
