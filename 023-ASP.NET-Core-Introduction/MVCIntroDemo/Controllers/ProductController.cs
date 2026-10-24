using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MVCIntroDemo.Models;

namespace MVCIntroDemo.Controllers;

public class ProductController : Controller
{
    private static readonly IReadOnlyList<ProductViewModel> Products =
    [
        new(1, "Cheese", 7.00m),
        new(2, "Ham", 5.50m),
        new(3, "Bread", 1.50m),
    ];

    [ActionName("My-Products")]
    public IActionResult All(string? keyword)
    {
        var products = string.IsNullOrWhiteSpace(keyword)
            ? Products
            : Products.Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        return View("All", products);
    }

    public IActionResult ById(int id) =>
        Products.FirstOrDefault(p => p.Id == id) is { } product ? View(product) : NotFound();

    public IActionResult AllAsJson() => Json(Products);

    public IActionResult AllAsText() => Content(ProductsText(), "text/plain");

    // The course version added the Content-Disposition header by hand; File(..., fileDownloadName) sets it correctly.
    public IActionResult AllAsTextFile() =>
        File(Encoding.UTF8.GetBytes(ProductsText()), "text/plain", "products.txt");

    private static string ProductsText() =>
        string.Join(Environment.NewLine, Products.Select(p =>
            string.Create(CultureInfo.InvariantCulture, $"Product {p.Id}: {p.Name} - {p.Price:F2} lv.")));
}
