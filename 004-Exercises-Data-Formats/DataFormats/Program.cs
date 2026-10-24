using DataFormats;

static string Read(string file) => File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Datasets", file));

Console.WriteLine("== Books (JSON) ==");
Console.WriteLine(JsonParser.FormatBooks(JsonParser.ParseBooks(Read("books.json"))));
Console.WriteLine("== Students (JSON) ==");
Console.WriteLine(JsonParser.FormatStudents(JsonParser.ParseStudents(Read("students.json"))));
Console.WriteLine("== Devices (XML -> JSON) ==");
Console.WriteLine(XmlToJson.DevicesToJson(Read("devices.xml")));
Console.WriteLine("== Vehicles (XML -> JSON) ==");
Console.WriteLine(XmlToJson.VehiclesToJson(Read("vehicles.xml")));
Console.WriteLine("== Orders (YAML -> HTML) ==");
Console.WriteLine(YamlToHtml.OrdersToHtml(Read("orders.yaml")));
Console.WriteLine("== Reservations (YAML -> HTML) ==");
Console.WriteLine(YamlToHtml.ReservationsToHtml(Read("reservations.yaml")));
