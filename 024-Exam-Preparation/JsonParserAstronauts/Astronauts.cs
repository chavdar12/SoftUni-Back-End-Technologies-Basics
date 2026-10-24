using System.Text;
using System.Text.Json;

namespace JsonParserAstronauts;

public record Missions(string? Current, List<string> Past);

public record Astronaut(int AstronautId, string AstronautName, int Age, string Profession, int YearsExperience, Missions Missions);

public static class AstronautsParser
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static List<Astronaut> Parse(string json) =>
        JsonSerializer.Deserialize<List<Astronaut>>(json, Options) ?? throw new JsonException("No astronauts data found.");

    public static string Format(IEnumerable<Astronaut> astronauts)
    {
        var sb = new StringBuilder("Astronauts:\n");
        var number = 1;
        foreach (var a in astronauts)
        {
            sb.Append($"Astronaut #{number++}\n");
            sb.Append($"ID: {a.AstronautId} Name: {a.AstronautName}, Age: {a.Age}, Profession: {a.Profession}, Experience: {a.YearsExperience} years\n");
            sb.Append($"Current mission: {a.Missions.Current ?? "none"}\n");
            sb.Append($"Past missions: {(a.Missions.Past.Count > 0 ? string.Join(", ", a.Missions.Past) : "none")}\n");
        }
        return sb.ToString();
    }

    public static string ParseAndFormat(string json)
    {
        try
        {
            return Format(Parse(json));
        }
        catch (JsonException e)
        {
            return $"JSON Parsing Error: {e.Message}";
        }
    }
}
