using System.Xml.Serialization;

namespace DataFormats;

public record Book(string Title, string Author, string Isbn, int PublishedYear, List<string> Genres, bool Available);

public record Course(string Name, double Grade);

public record Student(int Id, string Name, List<Course> Courses);

public class OrderItem
{
    public string Product { get; set; } = "";
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class Order
{
    public int OrderId { get; set; }
    public string Customer { get; set; } = "";
    public DateOnly Date { get; set; }
    public List<OrderItem> Items { get; set; } = [];

    public decimal Total => Items.Sum(i => i.Quantity * i.Price);
}

public class Reservation
{
    public string ReservationId { get; set; } = "";
    public string Guest { get; set; } = "";
    public int Room { get; set; }
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }

    public int Nights => CheckOut.DayNumber - CheckIn.DayNumber;
}

[XmlRoot("devices")]
public class DeviceList
{
    [XmlElement("device")]
    public List<Device> Items { get; set; } = [];
}

public class Device
{
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlElement("name")]
    public string Name { get; set; } = "";

    [XmlElement("type")]
    public string Type { get; set; } = "";

    [XmlElement("price")]
    public decimal Price { get; set; }
}

[XmlRoot("vehicles")]
public class VehicleList
{
    [XmlElement("vehicle")]
    public List<Vehicle> Items { get; set; } = [];
}

public class Vehicle
{
    [XmlElement("make")]
    public string Make { get; set; } = "";

    [XmlElement("model")]
    public string Model { get; set; } = "";

    [XmlElement("year")]
    public int Year { get; set; }
}
