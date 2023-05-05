using System.Text.Json.Serialization;

namespace Elefess.Core.Models;

public sealed record LfsRequestObjectRef(
    [property: JsonPropertyName("name")] 
        string Name);