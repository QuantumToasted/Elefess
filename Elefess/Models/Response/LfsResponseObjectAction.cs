using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// 
/// </summary>
/// <param name="Uri">The URL the <c>git-lfs</c> client will make a POST (upload) or GET (download) request to.</param>
/// <param name="Headers">Optional headers to accompany the POST or GET request to <see cref="Uri"/>.</param>
/// <param name="ExpiryInSeconds">The number of seconds from now until this action is no longer valid.</param>
/// <param name="ExpiresAt">The exact date and time this action will no longer be valid.</param>
public sealed record LfsResponseObjectAction(
    [property: JsonPropertyName("href")]
        Uri Uri,
    [property: JsonPropertyName("header"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IReadOnlyDictionary<string, string>? Headers = null,
    [property: JsonPropertyName("expires_in"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        int? ExpiryInSeconds = null,
    [property: JsonPropertyName("expires_at"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        DateTimeOffset? ExpiresAt = null);