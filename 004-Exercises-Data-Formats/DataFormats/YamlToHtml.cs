using System.Globalization;
using System.Net;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DataFormats;

public static class YamlToHtml
{
    private static readonly IDeserializer Deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    public static List<T> Parse<T>(string yaml) => Deserializer.Deserialize<List<T>>(yaml) ?? [];

    public static string OrdersToHtml(string yaml) => Table(
        ["Order", "Customer", "Date", "Items", "Total"],
        Parse<Order>(yaml).Select(o => new[]
        {
            o.OrderId.ToString(CultureInfo.InvariantCulture),
            o.Customer,
            o.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            string.Join(", ", o.Items.Select(i => $"{i.Product} x{i.Quantity}")),
            o.Total.ToString("F2", CultureInfo.InvariantCulture),
        }));

    public static string ReservationsToHtml(string yaml) => Table(
        ["Reservation", "Guest", "Room", "Check-in", "Nights"],
        Parse<Reservation>(yaml).Select(r => new[]
        {
            r.ReservationId,
            r.Guest,
            r.Room.ToString(CultureInfo.InvariantCulture),
            r.CheckIn.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            r.Nights.ToString(CultureInfo.InvariantCulture),
        }));

    private static string Table(string[] headers, IEnumerable<string[]> rows)
    {
        var sb = new StringBuilder("<table>\n  <tr>");
        foreach (var header in headers)
        {
            sb.Append($"<th>{header}</th>");
        }
        sb.Append("</tr>\n");
        foreach (var row in rows)
        {
            sb.Append("  <tr>");
            foreach (var cell in row)
            {
                sb.Append($"<td>{WebUtility.HtmlEncode(cell)}</td>");
            }
            sb.Append("</tr>\n");
        }
        return sb.Append("</table>").ToString();
    }
}
