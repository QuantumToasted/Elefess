using System.Text.Json;
using System.Text.Json.Serialization;

namespace Elefess.Models;

internal sealed class LfsResponseObjectCollectionJsonConverter : JsonConverter<IReadOnlyCollection<LfsResponseObject>>
{
    public override IReadOnlyCollection<LfsResponseObject> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var objects = new List<LfsResponseObject>();
        
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Invalid array start.");

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            var document = JsonDocument.ParseValue(ref reader);

            if (document.RootElement.TryGetProperty("error", out _))
            {
                objects.Add(document.RootElement.Deserialize<LfsResponseErrorObject>()!);
            }
            else
            {
                objects.Add(document.RootElement.Deserialize<LfsResponseDataObject>()!);
            }
        }
        
        return objects;
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<LfsResponseObject> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}