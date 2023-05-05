using System.Text.Json.Serialization;

namespace Elefess.Core.Models;

public sealed record LfsResponseObjectAction(
    [property: JsonPropertyName("href")]
        Uri Uri,
    [property: JsonPropertyName("header"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IReadOnlyDictionary<string, string>? Headers = null,
    [property: JsonPropertyName("expires_in"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        int? ExpiryInSeconds = null,
    [property: JsonPropertyName("expires_at"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        DateTimeOffset? ExpiresAt = null);