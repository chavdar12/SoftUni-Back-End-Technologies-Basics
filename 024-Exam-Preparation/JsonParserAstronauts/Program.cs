using JsonParserAstronauts;

var path = Path.Combine(AppContext.BaseDirectory, "Datasets", "Astronauts.json");
Console.Write(AstronautsParser.ParseAndFormat(File.ReadAllText(path)));
