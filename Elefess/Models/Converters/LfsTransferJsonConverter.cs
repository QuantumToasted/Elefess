using System.Text.Json;
using System.Text.Json.Serialization;

namespace Elefess.Models;

internal sealed class LfsTransferJsonConverter : JsonConverter<LfsTransferAdapter>
{
    public override LfsTransferAdapter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString()!;

    public override void Write(Utf8JsonWriter writer, LfsTransferAdapter value, JsonSerializerOptions options)
        => writer.WriteStringValue(value);
}