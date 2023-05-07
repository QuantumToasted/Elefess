using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// A Git LFS API error response model. Returned when a non-object or other unhandled error occurs.
/// </summary>
/// <param name="Message">A message with details about the error or problem that occurred.</param>
/// <param name="DocumentationUri">A URL to documentation for the custom Git LFS server.</param>
/// <param name="RequestId">The ID of the original request.</param>
/// <remarks>
/// <see cref="ILfsAuthenticator"/>, <see cref="ILfsRequestValidator"/>, <see cref="ILfsOidMapper"/>, <see cref="ILfsTransferSelector"/>,
/// and <see cref="ILfsObjectManager"/> throwing <see cref="Exception"/>s will generate this response automatically.
/// </remarks>
public sealed record LfsErrorResponse(
    [property: JsonPropertyName("message")]
        string Message,
    [property: JsonPropertyName("documentation_url"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        Uri? DocumentationUri = null,
    [property: JsonPropertyName("request_id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        string? RequestId = null);