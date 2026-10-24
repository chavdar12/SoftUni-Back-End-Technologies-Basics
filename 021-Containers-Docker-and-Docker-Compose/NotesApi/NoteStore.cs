using System.Collections.Concurrent;

namespace NotesApi;

public record Note(int Id, string Text, DateTimeOffset CreatedOn);

public record CreateNoteRequest(string? Text);

// shortcut: notes live in memory and vanish when the container restarts; swap for a database volume if they must persist.
public class NoteStore(TimeProvider timeProvider)
{
    public const int MaxTextLength = 200;

    private readonly ConcurrentDictionary<int, Note> _notes = new();
    private int _lastId;

    public IEnumerable<Note> All() => _notes.Values.OrderBy(n => n.Id);

    public Note? Get(int id) => _notes.GetValueOrDefault(id);

    public Note Add(string text)
    {
        var note = new Note(Interlocked.Increment(ref _lastId), text.Trim(), timeProvider.GetUtcNow());
        _notes[note.Id] = note;
        return note;
    }

    public bool Delete(int id) => _notes.TryRemove(id, out _);

    public static string? Validate(string? text) =>
        string.IsNullOrWhiteSpace(text) ? "Text is required."
        : text.Trim().Length > MaxTextLength ? $"Text must be at most {MaxTextLength} characters."
        : null;
}
