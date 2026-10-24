using NotesApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<NoteStore>();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

var notes = app.MapGroup("/api/notes");
notes.MapGet("/", (NoteStore store) => store.All());
notes.MapGet("/{id:int}", (int id, NoteStore store) =>
    store.Get(id) is { } note ? Results.Ok(note) : Results.NotFound());
notes.MapPost("/", (CreateNoteRequest request, NoteStore store) =>
{
    if (NoteStore.Validate(request.Text) is { } error)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]> { ["text"] = [error] });
    }
    var note = store.Add(request.Text!);
    return Results.Created($"/api/notes/{note.Id}", note);
});
notes.MapDelete("/{id:int}", (int id, NoteStore store) =>
    store.Delete(id) ? Results.NoContent() : Results.NotFound());

app.Run();

public partial class Program;
