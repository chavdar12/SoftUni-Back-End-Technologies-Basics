using System.Globalization;
using System.Text;
using System.Text.Json;

namespace JsonParserMusicians;

public record InstrumentSkill(string InstrumentName, double ExperienceLevel);

public record Musician(int ParticipantId, string ParticipantName, DateOnly DateOfBirth, List<InstrumentSkill> InstrumentSkills, List<string> PreferredGenres);

public static class MusiciansParser
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static List<Musician> Parse(string json) =>
        JsonSerializer.Deserialize<List<Musician>>(json, Options) ?? throw new JsonException("No musicians data found.");

    public static string Format(IEnumerable<Musician> musicians)
    {
        var sb = new StringBuilder("Musicians:\n");
        var number = 1;
        foreach (var musician in musicians)
        {
            sb.Append($"Musician #{number++}\n");
            sb.Append($"ID: {musician.ParticipantId} Name: {musician.ParticipantName}, Date of Birth: {musician.DateOfBirth:yyyy-MM-dd}\n");
            var instrument = 1;
            foreach (var skill in musician.InstrumentSkills)
            {
                sb.Append(string.Create(CultureInfo.InvariantCulture,
                    $"Instrument #{instrument++}: {skill.InstrumentName}, Skill level: {skill.ExperienceLevel}\n"));
            }
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
