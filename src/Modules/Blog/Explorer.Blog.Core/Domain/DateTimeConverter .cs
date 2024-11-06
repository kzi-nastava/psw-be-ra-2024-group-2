using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly string _format = "yyyy-MM-ddTHH:mm:ss.fffffffZ"; 

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String &&
            DateTime.TryParseExact(reader.GetString(), _format, null, System.Globalization.DateTimeStyles.RoundtripKind, out var date))
        {
            return date;
        }

        throw new JsonException($"Invalid date format. Expected format: {_format}");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format));
    }
}
