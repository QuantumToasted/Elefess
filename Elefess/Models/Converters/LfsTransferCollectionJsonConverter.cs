using System.Text.Json;
using System.Text.Json.Serialization;

namespace Elefess.Models;

internal sealed class LfsTransferCollectionJsonConverter : JsonConverter<ICollection<LfsTransferAdapter>>
{
    public override ICollection<LfsTransferAdapter> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var transfers = new List<LfsTransferAdapter>();
        
        if (reader.TokenType != JsonTokenType.StartArray || !reader.Read())
            throw new JsonException("Invalid array start, or failed to read array start.");

        while (reader.TokenType != JsonTokenType.EndArray)
        {
            transfers.Add(reader.GetString()!);
            reader.Read();
        }

        return transfers;
    }

    public override void Write(Utf8JsonWriter writer, ICollection<LfsTransferAdapter> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        
        foreach (var transfer in value)
        {
            writer.WriteStringValue(transfer.Type);
        }
        
        writer.WriteEndArray();
    }
}