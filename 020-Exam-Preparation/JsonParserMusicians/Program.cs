using JsonParserMusicians;

var path = Path.Combine(AppContext.BaseDirectory, "Datasets", "Musicians.json");
Console.Write(MusiciansParser.ParseAndFormat(File.ReadAllText(path)));
