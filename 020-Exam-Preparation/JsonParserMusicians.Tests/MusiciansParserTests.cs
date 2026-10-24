namespace JsonParserMusicians.Tests;

public class MusiciansParserTests
{
    private static string Dataset() =>
        File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Datasets", "Musicians.json"));

    [Test]
    public void Parse_ReadsAllMusicians()
    {
        var musicians = MusiciansParser.Parse(Dataset());

        Assert.That(musicians, Has.Count.EqualTo(3));
        Assert.That(musicians[0].DateOfBirth, Is.EqualTo(new DateOnly(1988, 3, 21)));
        Assert.That(musicians[0].InstrumentSkills[1], Is.EqualTo(new InstrumentSkill("Piano", 3.6)));
        Assert.That(musicians[1].PreferredGenres, Is.EqualTo(new[] { "Rock" }));
    }

    [Test]
    public void Format_PrintsMusiciansAndNumberedInstruments()
    {
        var output = MusiciansParser.Format(MusiciansParser.Parse(Dataset()));

        Assert.That(output, Does.StartWith("Musicians:\nMusician #1\nID: 101 Name: Emma Johnson, Date of Birth: 1988-03-21\n"));
        Assert.That(output, Does.Contain("Instrument #2: Piano, Skill level: 3.6\n"));
        Assert.That(output, Does.Contain("Musician #3\nID: 103 Name: Sofia Petrova, Date of Birth: 2001-11-05\n"));
    }

    [Test]
    public void Format_EmptyList_PrintsHeaderOnly() => Assert.That(MusiciansParser.Format([]), Is.EqualTo("Musicians:\n"));

    [TestCase("{ broken")]
    [TestCase("null")]
    [TestCase("{\"participantId\": 1}")]
    public void ParseAndFormat_InvalidJson_ReturnsError(string json) =>
        Assert.That(MusiciansParser.ParseAndFormat(json), Does.StartWith("JSON Parsing Error:"));
}
