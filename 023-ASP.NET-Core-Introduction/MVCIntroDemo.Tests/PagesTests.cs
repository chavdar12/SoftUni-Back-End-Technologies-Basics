using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MVCIntroDemo.Tests;

// Renders the real Razor views through the HTTP pipeline, so broken views or routes fail the build.
public class PagesTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task Home_RendersGreeting()
    {
        var html = await _client.GetStringAsync("/");

        Assert.That(html, Does.Contain("<h1>Hello World!</h1>"));
    }

    [Test]
    public async Task MyProducts_RendersFilteredList()
    {
        var html = await _client.GetStringAsync("/Product/My-Products?keyword=ham");

        Assert.That(html, Does.Contain("Ham</a> - 5.50 lv."));
        Assert.That(html, Does.Not.Contain("Cheese</a>"));
    }

    [Test]
    public async Task ById_RendersProductOr404()
    {
        Assert.That(await _client.GetStringAsync("/Product/ById/1"), Does.Contain("<h1>Cheese</h1>"));
        Assert.That((await _client.GetAsync("/Product/ById/42")).StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task AllAsTextFile_SetsContentDisposition()
    {
        var response = await _client.GetAsync("/Product/AllAsTextFile");

        Assert.That(response.Content.Headers.ContentDisposition!.FileName, Is.EqualTo("products.txt"));
    }
}
