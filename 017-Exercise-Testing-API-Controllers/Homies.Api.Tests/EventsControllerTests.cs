using System.Net;
using System.Net.Http.Json;
using Homies.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;

namespace Homies.Api.Tests;

// Drives the real HTTP pipeline (routing, model binding, validation, status codes) against a throwaway database.
public class EventsControllerTests
{
    private SqliteConnection _keepAlive = null!;
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        // A named shared-cache in-memory database lives as long as one connection to it stays open.
        var connectionString = $"Data Source=homies-{Guid.NewGuid()};Mode=Memory;Cache=Shared";
        _keepAlive = new SqliteConnection(connectionString);
        _keepAlive.Open();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseSetting("ConnectionStrings:Homies", connectionString));
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
        _keepAlive.Dispose();
    }

    private static object Form(string name = "Board games", int typeId = 2) => new
    {
        name,
        description = "Bring your favourite game",
        start = new DateTime(2026, 6, 10, 18, 0, 0),
        end = new DateTime(2026, 6, 10, 22, 0, 0),
        typeId,
    };

    private Task<HttpResponseMessage> Send(HttpMethod method, string url, string? userId = null, object? body = null)
    {
        var request = new HttpRequestMessage(method, url);
        if (userId != null)
        {
            request.Headers.Add("X-User-Id", userId);
        }
        if (body != null)
        {
            request.Content = JsonContent.Create(body);
        }
        return _client.SendAsync(request);
    }

    private async Task<int> CreateEvent(string userId = "organiser", string name = "Board games")
    {
        var response = await Send(HttpMethod.Post, "/api/events", userId, Form(name));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        return (await response.Content.ReadFromJsonAsync<CreatedResponse>())!.Id;
    }

    private sealed record CreatedResponse(int Id);

    [Test]
    public async Task GetAll_EmptyDatabase_ReturnsEmptyArray()
    {
        var events = await _client.GetFromJsonAsync<List<EventSummary>>("/api/events");

        Assert.That(events, Is.Empty);
    }

    [Test]
    public async Task GetTypes_ReturnsSeededTypes()
    {
        var types = await _client.GetFromJsonAsync<List<TypeViewModel>>("/api/events/types");

        Assert.That(types!.Select(t => t.Name), Is.EqualTo(new[] { "Animals", "Fun", "Discussion", "Work" }));
    }

    [Test]
    public async Task Post_ValidEvent_Returns201WithLocation()
    {
        var response = await Send(HttpMethod.Post, "/api/events", "organiser", Form());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(response.Headers.Location!.AbsolutePath, Does.Match(@"^/api/events/\d+$"));

        var details = await _client.GetFromJsonAsync<EventDetails>(response.Headers.Location);
        Assert.That(details!.Name, Is.EqualTo("Board games"));
        Assert.That(details.OrganiserId, Is.EqualTo("organiser"));
    }

    [Test]
    public async Task Post_InvalidModel_Returns400()
    {
        var response = await Send(HttpMethod.Post, "/api/events", "organiser", Form(name: "Bad"));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(await response.Content.ReadAsStringAsync(), Does.Contain("Name"));
    }

    [Test]
    public async Task Post_UnknownType_Returns400()
    {
        var response = await Send(HttpMethod.Post, "/api/events", "organiser", Form(typeId: 99));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(await response.Content.ReadAsStringAsync(), Does.Contain("TypeId"));
    }

    [Test]
    public async Task Post_WithoutUserHeader_Returns400()
    {
        var response = await Send(HttpMethod.Post, "/api/events", body: Form());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetDetails_MissingEvent_Returns404()
    {
        var response = await _client.GetAsync("/api/events/999");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Put_ByOrganiser_Returns204AndUpdates()
    {
        var id = await CreateEvent();

        var response = await Send(HttpMethod.Put, $"/api/events/{id}", "organiser", Form("Chess night", typeId: 3));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        var details = await _client.GetFromJsonAsync<EventDetails>($"/api/events/{id}");
        Assert.That(details!.Name, Is.EqualTo("Chess night"));
        Assert.That(details.Type, Is.EqualTo("Discussion"));
    }

    [Test]
    public async Task Put_ByAnotherUser_Returns403()
    {
        var id = await CreateEvent();

        var response = await Send(HttpMethod.Put, $"/api/events/{id}", "intruder", Form("Hijacked"));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task Put_MissingEvent_Returns404()
    {
        var response = await Send(HttpMethod.Put, "/api/events/999", "organiser", Form());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Join_ThenJoinAgain_Returns204Then409()
    {
        var id = await CreateEvent();

        var first = await Send(HttpMethod.Post, $"/api/events/{id}/join", "helper");
        var second = await Send(HttpMethod.Post, $"/api/events/{id}/join", "helper");

        Assert.That(first.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Assert.That(second.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task Join_MissingEvent_Returns404()
    {
        var response = await Send(HttpMethod.Post, "/api/events/999/join", "helper");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Joined_ListsOnlyEventsTheUserJoined()
    {
        var joinedId = await CreateEvent(name: "Joined event");
        await CreateEvent(name: "Other event");
        await Send(HttpMethod.Post, $"/api/events/{joinedId}/join", "helper");

        var response = await Send(HttpMethod.Get, "/api/events/joined", "helper");
        var events = await response.Content.ReadFromJsonAsync<List<EventSummary>>();

        Assert.That(events!.Select(e => e.Name), Is.EqualTo(new[] { "Joined event" }));
    }

    [Test]
    public async Task Leave_JoinedEvent_Returns204ThenNotFound()
    {
        var id = await CreateEvent();
        await Send(HttpMethod.Post, $"/api/events/{id}/join", "helper");

        var first = await Send(HttpMethod.Post, $"/api/events/{id}/leave", "helper");
        var second = await Send(HttpMethod.Post, $"/api/events/{id}/leave", "helper");

        Assert.That(first.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Assert.That(second.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
