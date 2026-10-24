using System.Text.Json;
using System.Xml.Linq;
using YamlDotNet.RepresentationModel;

namespace DataFormats.Lab.Tests;

// The lab is about writing well-formed data files; these tests prove each file parses and holds the expected shape.
public class DataFilesTests
{
    private static string DataPath(string file) => Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", file);

    [Test]
    public void ProductsJson_HasProductsWithNamePriceAndKeywords()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(DataPath("products.json")));
        var products = document.RootElement.EnumerateArray().ToList();

        Assert.That(products, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            foreach (var product in products)
            {
                Assert.That(product.GetProperty("name").GetString(), Is.Not.Empty);
                Assert.That(product.GetProperty("price").GetDecimal(), Is.GreaterThan(0));
                Assert.That(product.GetProperty("keywords").GetArrayLength(), Is.GreaterThan(0));
            }
        });
    }

    [Test]
    public void CitiesXml_HasCitiesWithLandmarks()
    {
        var cities = XDocument.Load(DataPath("cities.xml")).Root!.Elements("city").ToList();

        Assert.That(cities, Has.Count.EqualTo(3));
        Assert.That(cities.Select(c => c.Element("name")!.Value), Is.EqualTo(new[] { "Sofia", "Plovdiv", "Vienna" }));
        Assert.That(cities.All(c => c.Descendants("landmark").Any()), Is.True);
        Assert.That(int.Parse(cities[0].Element("population")!.Value), Is.GreaterThan(1_000_000));
    }

    [Test]
    public void CountriesYaml_HasCountriesWithCapitalsAndLanguages()
    {
        var yaml = new YamlStream();
        using (var reader = new StreamReader(DataPath("countries.yaml")))
        {
            yaml.Load(reader);
        }

        var root = (YamlMappingNode)yaml.Documents[0].RootNode;
        var countries = ((YamlSequenceNode)root.Children[new YamlScalarNode("countries")]).Cast<YamlMappingNode>().ToList();

        Assert.That(countries, Has.Count.EqualTo(3));
        var switzerland = countries.Single(c => c["name"].ToString() == "Switzerland");
        Assert.That(switzerland["capital"].ToString(), Is.EqualTo("Bern"));
        Assert.That(((YamlSequenceNode)switzerland["languages"]).Children, Has.Count.EqualTo(4));
    }
}
