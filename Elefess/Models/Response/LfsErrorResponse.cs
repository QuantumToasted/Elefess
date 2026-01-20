using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// A Git LFS API error response model. Returned when a non-object or other unhandled error occurs.
/// </summary>
/// <remarks>
/// <see cref="ILfsAuthenticator"/>, <see cref="ILfsRequestValidator"/>, <see cref="ILfsOidMapper"/>, <see cref="ILfsTransferSelector"/>,
/// and <see cref="ILfsObjectManager"/> throwing <see cref="Exception"/>s will generate this response automatically.
/// </remarks>
public sealed class LfsErrorResponse
{
    /// <summary>
    /// A message with details about the error or problem that occurred.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; init; }

    /// <summary>
    /// A URL to documentation for the custom Git LFS server.
    /// </summary>
    [JsonPropertyName("documentation_url"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Uri? DocumentationUri { get; init; }

    /// <summary>
    /// The ID of the original request.
    /// </summary>
    [JsonPropertyName("request_id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RequestId { get; init; }
}