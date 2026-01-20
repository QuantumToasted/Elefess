using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// The details of a given Git LFS action (such as the URL and headers) when making a request for a file.
/// </summary>
public sealed class LfsResponseObjectAction
{
    /// <summary>
    /// The URL the <c>git-lfs</c> client will make a POST (upload) or GET (download) request to.
    /// </summary>
    [JsonPropertyName("href")]
    public required Uri Uri { get; init; }

    /// <summary>
    /// Optional headers to accompany the POST or GET request to <see cref="Uri"/>.
    /// </summary>
    [JsonPropertyName("header"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyDictionary<string, string>? Headers { get; init; }

    /// <summary>
    /// The number of seconds from now until this action is no longer valid.
    /// </summary>
    [JsonPropertyName("expires_in"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ExpiryInSeconds { get; init; }

    /// <summary>
    /// The exact date and time this action will no longer be valid.
    /// </summary>
    [JsonPropertyName("expires_at"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? ExpiresAt { get; init; }
}