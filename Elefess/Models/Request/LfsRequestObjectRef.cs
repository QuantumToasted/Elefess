using System.Text.Json.Serialization;

namespace Elefess.Models;

public sealed record LfsRequestObjectRef(
    [property: JsonPropertyName("name")] 
        string Name);