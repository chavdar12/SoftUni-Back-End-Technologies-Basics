using System.Globalization;
using System.Text;
using System.Text.Json;

namespace DataFormats;

public static class JsonParser
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static List<Book> ParseBooks(string json) =>
        JsonSerializer.Deserialize<List<Book>>(json, Options) ?? throw new JsonException("Expected an array of books.");

    public static List<Student> ParseStudents(string json) =>
        JsonSerializer.Deserialize<List<Student>>(json, Options) ?? throw new JsonException("Expected an array of students.");

    public static string FormatBooks(IEnumerable<Book> books)
    {
        var sb = new StringBuilder();
        foreach (var book in books)
        {
            sb.AppendLine($"{book.Title} by {book.Author} ({book.PublishedYear})");
            sb.AppendLine($"  ISBN: {book.Isbn}, Genres: {string.Join(", ", book.Genres)}, Available: {(book.Available ? "yes" : "no")}");
        }
        return sb.ToString();
    }

    public static string FormatStudents(IEnumerable<Student> students)
    {
        var sb = new StringBuilder();
        foreach (var student in students)
        {
            var average = student.Courses.Count == 0 ? 0 : student.Courses.Average(c => c.Grade);
            sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"#{student.Id} {student.Name} - average grade {average:F2}"));
            foreach (var course in student.Courses)
            {
                sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"  {course.Name}: {course.Grade:F2}"));
            }
        }
        return sb.ToString();
    }
}
