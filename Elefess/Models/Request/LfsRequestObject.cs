using System.Text.Json.Serialization;

namespace Elefess.Models;

public sealed record LfsRequestObject(
    [property: JsonPropertyName("oid")]
        string Oid, 
    [property: JsonPropertyName("size")]
        long Size);