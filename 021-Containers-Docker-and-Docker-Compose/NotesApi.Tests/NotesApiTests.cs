using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace NotesApi.Tests;

public class NotesApiTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task Health_ReturnsHealthy()
    {
        var response = await _client.GetAsync("/health");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(await response.Content.ReadAsStringAsync(), Is.EqualTo("Healthy"));
    }

    [Test]
    public async Task GetAll_Initially_ReturnsEmpty()
    {
        Assert.That(await _client.GetFromJsonAsync<List<Note>>("/api/notes"), Is.Empty);
    }

    [Test]
    public async Task Post_ValidNote_Returns201AndCanBeFetched()
    {
        var response = await _client.PostAsJsonAsync("/api/notes", new CreateNoteRequest("  Buy milk  "));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        var created = await response.Content.ReadFromJsonAsync<Note>();
        Assert.That(created!.Text, Is.EqualTo("Buy milk"));

        var fetched = await _client.GetFromJsonAsync<Note>(response.Headers.Location);
        Assert.That(fetched, Is.EqualTo(created));
    }

    [Test]
    public async Task Post_AssignsIncreasingIds()
    {
        await _client.PostAsJsonAsync("/api/notes", new CreateNoteRequest("First"));
        await _client.PostAsJsonAsync("/api/notes", new CreateNoteRequest("Second"));

        var all = await _client.GetFromJsonAsync<List<Note>>("/api/notes");

        Assert.That(all!.Select(n => (n.Id, n.Text)), Is.EqualTo(new[] { (1, "First"), (2, "Second") }));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public async Task Post_EmptyText_Returns400(string? text)
    {
        var response = await _client.PostAsJsonAsync("/api/notes", new CreateNoteRequest(text));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Post_TooLongText_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/api/notes", new CreateNoteRequest(new string('x', 201)));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetById_Missing_Returns404()
    {
        Assert.That((await _client.GetAsync("/api/notes/42")).StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Delete_ExistingThenAgain_Returns204Then404()
    {
        await _client.PostAsJsonAsync("/api/notes", new CreateNoteRequest("Temp"));

        var first = await _client.DeleteAsync("/api/notes/1");
        var second = await _client.DeleteAsync("/api/notes/1");

        Assert.That(first.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Assert.That(second.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
