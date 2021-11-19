using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MedicDate.Utility.JsonCoverters;

public class CustomDateTimeJsonConverter : JsonConverter<DateTime>
{
  public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert,
    JsonSerializerOptions options)
  {
    return DateTime.ParseExact(reader.GetString()!, "MM/dd/yyyy HH:mm:ss",
      CultureInfo.InvariantCulture);
  }

  public override void Write(Utf8JsonWriter writer, DateTime value,
    JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.ToString(
      "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
  }
}