using System.Text.Json;
using System.Xml.Serialization;

namespace DataFormats;

public static class XmlToJson
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    public static T Deserialize<T>(string xml)
    {
        using var reader = new StringReader(xml);
        return (T)(new XmlSerializer(typeof(T)).Deserialize(reader)
            ?? throw new InvalidOperationException($"Could not read {typeof(T).Name} from XML."));
    }

    public static string DevicesToJson(string xml) => JsonSerializer.Serialize(Deserialize<DeviceList>(xml).Items, Options);

    public static string VehiclesToJson(string xml) => JsonSerializer.Serialize(Deserialize<VehicleList>(xml).Items, Options);
}
