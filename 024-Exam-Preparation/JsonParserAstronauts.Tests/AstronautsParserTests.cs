namespace JsonParserAstronauts.Tests;

public class AstronautsParserTests
{
    private static string Dataset() =>
        File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Datasets", "Astronauts.json"));

    [Test]
    public void Parse_ReadsNestedMissions()
    {
        var astronauts = AstronautsParser.Parse(Dataset());

        Assert.That(astronauts, Has.Count.EqualTo(3));
        Assert.That(astronauts[0].Missions.Current, Is.EqualTo("Europa Exploration"));
        Assert.That(astronauts[0].Missions.Past, Is.EqualTo(new[] { "Lunar Research", "Solar Flare Study" }));
        Assert.That(astronauts[2].Missions.Current, Is.Null);
    }

    [Test]
    public void Format_PrintsEveryAstronaut()
    {
        var output = AstronautsParser.Format(AstronautsParser.Parse(Dataset()));

        Assert.That(output, Does.StartWith(
            "Astronauts:\nAstronaut #1\nID: 1001 Name: Rachel Nguyen, Age: 29, Profession: Biologist, Experience: 7 years\n" +
            "Current mission: Europa Exploration\nPast missions: Lunar Research, Solar Flare Study\n"));
        Assert.That(output, Does.Contain("Astronaut #3\n"));
    }

    [Test]
    public void Format_NoMissions_PrintsNone()
    {
        var output = AstronautsParser.Format([new Astronaut(1, "New Recruit", 22, "Cadet", 0, new Missions(null, []))]);

        Assert.That(output, Does.EndWith("Current mission: none\nPast missions: none\n"));
    }

    [TestCase("[{")]
    [TestCase("null")]
    public void ParseAndFormat_InvalidJson_ReturnsError(string json) =>
        Assert.That(AstronautsParser.ParseAndFormat(json), Does.StartWith("JSON Parsing Error:"));
}
