using System.Text.Json;

namespace DataFormats.Tests;

public class ConverterTests
{
    private static string Dataset(string file) => File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Datasets", file));

    [Test]
    public void ParseBooks_ReadsAllFields()
    {
        var books = JsonParser.ParseBooks(Dataset("books.json"));

        Assert.That(books, Has.Count.EqualTo(2));
        Assert.That(books[0], Is.EqualTo(books[0] with { Title = "Clean Code", PublishedYear = 2008, Available = true }));
        Assert.That(books[1].Genres, Is.EqualTo(new[] { "Programming" }));
    }

    [Test]
    public void FormatBooks_PrintsTitleAuthorAndAvailability()
    {
        var output = JsonParser.FormatBooks(JsonParser.ParseBooks(Dataset("books.json")));

        Assert.That(output, Does.Contain("Clean Code by Robert C. Martin (2008)"));
        Assert.That(output, Does.Contain("Available: no"));
    }

    [Test]
    public void FormatStudents_PrintsAverageGrade()
    {
        var output = JsonParser.FormatStudents(JsonParser.ParseStudents(Dataset("students.json")));

        Assert.That(output, Does.Contain("#1 Maria Ivanova - average grade 5.88"));
        Assert.That(output, Does.Contain("#2 Georgi Petrov - average grade 4.50"));
    }

    [Test]
    public void FormatStudents_StudentWithoutCourses_HasZeroAverage()
    {
        var output = JsonParser.FormatStudents([new Student(3, "New Student", [])]);

        Assert.That(output, Does.Contain("average grade 0.00"));
    }

    [Test]
    public void ParseBooks_InvalidJson_Throws()
    {
        Assert.Throws<JsonException>(() => JsonParser.ParseBooks("{ not json"));
        Assert.Throws<JsonException>(() => JsonParser.ParseBooks("null"));
    }

    [Test]
    public void DevicesToJson_ConvertsAttributesAndElements()
    {
        using var json = JsonDocument.Parse(XmlToJson.DevicesToJson(Dataset("devices.xml")));
        var first = json.RootElement[0];

        Assert.That(json.RootElement.GetArrayLength(), Is.EqualTo(2));
        Assert.That(first.GetProperty("id").GetInt32(), Is.EqualTo(1));
        Assert.That(first.GetProperty("name").GetString(), Is.EqualTo("iPhone 17"));
        Assert.That(first.GetProperty("price").GetDecimal(), Is.EqualTo(999.00m));
    }

    [Test]
    public void VehiclesToJson_ConvertsEveryVehicle()
    {
        using var json = JsonDocument.Parse(XmlToJson.VehiclesToJson(Dataset("vehicles.xml")));

        Assert.That(json.RootElement.EnumerateArray().Select(v => v.GetProperty("make").GetString()),
            Is.EqualTo(new[] { "Toyota", "Skoda" }));
        Assert.That(json.RootElement[1].GetProperty("year").GetInt32(), Is.EqualTo(2019));
    }

    [Test]
    public void VehiclesToJson_EmptyRoot_ReturnsEmptyArray()
    {
        Assert.That(XmlToJson.VehiclesToJson("<vehicles />"), Is.EqualTo("[]"));
    }

    [Test]
    public void OrdersToHtml_RendersRowsWithTotals()
    {
        var html = YamlToHtml.OrdersToHtml(Dataset("orders.yaml"));

        Assert.That(html, Does.StartWith("<table>"));
        Assert.That(html, Does.Contain("<th>Total</th>"));
        Assert.That(html, Does.Contain("<td>1001</td><td>Ivan Georgiev</td><td>2026-03-14</td><td>Laptop x1, Mouse x2</td><td>1538.99</td>"));
        Assert.That(html, Does.Contain("<td>329.00</td>"));
    }

    [Test]
    public void ReservationsToHtml_RendersNights()
    {
        var html = YamlToHtml.ReservationsToHtml(Dataset("reservations.yaml"));

        Assert.That(html, Does.Contain("<td>R-001</td><td>Petar Stoyanov</td><td>204</td><td>2026-07-01</td><td>4</td>"));
        Assert.That(html, Does.Contain("<td>R-002</td>"));
    }

    [Test]
    public void ReservationsToHtml_EncodesHtml()
    {
        const string yaml = "- reservationId: X\n  guest: \"<script>\"\n  room: 1\n  checkIn: 2026-01-01\n  checkOut: 2026-01-02\n";

        Assert.That(YamlToHtml.ReservationsToHtml(yaml), Does.Contain("<td>&lt;script&gt;</td>"));
    }
}
