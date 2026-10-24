using Microsoft.AspNetCore.Mvc;
using MVCIntroDemo.Controllers;
using MVCIntroDemo.Models;

namespace MVCIntroDemo.Tests;

public class ProductControllerTests
{
    private ProductController _controller = null!;

    [SetUp]
    public void SetUp() => _controller = new ProductController();

    [TearDown]
    public void TearDown() => _controller.Dispose();

    [Test]
    public void All_WithoutKeyword_ReturnsAllProducts()
    {
        var result = (ViewResult)_controller.All(null);

        Assert.That(result.ViewName, Is.EqualTo("All"));
        Assert.That(((IEnumerable<ProductViewModel>)result.Model!).Select(p => p.Name), Is.EqualTo(new[] { "Cheese", "Ham", "Bread" }));
    }

    [TestCase("bREad", new[] { "Bread" })]
    [TestCase("a", new[] { "Ham", "Bread" })]
    [TestCase("pizza", new string[0])]
    public void All_WithKeyword_FiltersCaseInsensitively(string keyword, string[] expected)
    {
        var result = (ViewResult)_controller.All(keyword);

        Assert.That(((IEnumerable<ProductViewModel>)result.Model!).Select(p => p.Name), Is.EqualTo(expected));
    }

    [Test]
    public void ById_ExistingProduct_ReturnsView()
    {
        var result = (ViewResult)_controller.ById(2);

        Assert.That(result.Model, Is.EqualTo(new ProductViewModel(2, "Ham", 5.50m)));
    }

    [Test]
    public void ById_MissingProduct_ReturnsNotFound() => Assert.That(_controller.ById(99), Is.InstanceOf<NotFoundResult>());

    [Test]
    public void AllAsJson_ReturnsAllProducts()
    {
        var result = (JsonResult)_controller.AllAsJson();

        Assert.That(((IEnumerable<ProductViewModel>)result.Value!).Count(), Is.EqualTo(3));
    }

    [Test]
    public void AllAsText_FormatsEveryProduct()
    {
        var result = (ContentResult)_controller.AllAsText();

        Assert.That(result.ContentType, Is.EqualTo("text/plain"));
        Assert.That(result.Content!.Split(Environment.NewLine), Is.EqualTo(new[]
        {
            "Product 1: Cheese - 7.00 lv.",
            "Product 2: Ham - 5.50 lv.",
            "Product 3: Bread - 1.50 lv.",
        }));
    }

    [Test]
    public void AllAsTextFile_ReturnsDownload()
    {
        var result = (FileContentResult)_controller.AllAsTextFile();

        Assert.That(result.FileDownloadName, Is.EqualTo("products.txt"));
        Assert.That(result.ContentType, Is.EqualTo("text/plain"));
        Assert.That(System.Text.Encoding.UTF8.GetString(result.FileContents), Does.StartWith("Product 1: Cheese"));
    }
}
