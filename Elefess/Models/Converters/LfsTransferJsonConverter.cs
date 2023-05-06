using System.Text.Json;
using System.Text.Json.Serialization;

namespace Elefess.Models;

internal sealed class LfsTransferJsonConverter : JsonConverter<LfsTransfer>
{
    public override LfsTransfer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString()!;

    public override void Write(Utf8JsonWriter writer, LfsTransfer value, JsonSerializerOptions options)
        => writer.WriteStringValue(value);
}