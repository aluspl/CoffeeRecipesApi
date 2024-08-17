using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.App.Common.Converters;

public class JsonStringGuidConverter : JsonConverter<Guid>
{
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var success = reader.TryGetGuid(out var result);
        if (success)
        {
            return result;
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}