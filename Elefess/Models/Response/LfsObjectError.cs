using System.Net;
using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// The specific error model for a Git LFS response error object.
/// </summary>
public sealed class LfsObjectError
{
    /// <summary>
    /// The status code indicating the type of problem that occurred.
    /// </summary>
    [JsonPropertyName("code")]
    public required HttpStatusCode Code { get; init; }
    
    /// <summary>
    /// A message with details about the error or problem that occurred.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; init; }
    
    /// <summary>
    /// A <see cref="LfsObjectError"/> wrapping <see cref="HttpStatusCode.NotFound"/>.
    /// </summary>
    public static LfsObjectError NotFound(string message = "The object does not exist on the server")
        => new() { Code = HttpStatusCode.NotFound, Message = message };

    /// <summary>
    /// A <see cref="LfsObjectError"/> wrapping <see cref="HttpStatusCode.Conflict"/>.
    /// </summary>
    public static LfsObjectError Conflict(string message = "The specified hash algorithm disagrees with the server's acceptable options")
        => new() { Code = HttpStatusCode.Conflict, Message = message };

    /// <summary>
    /// A <see cref="LfsObjectError"/> wrapping <see cref="HttpStatusCode.Gone"/>.
    /// </summary>
    public static LfsObjectError Gone(string message = "The object was removed by the owner")
        => new() { Code = HttpStatusCode.Gone, Message = message };

    /// <summary>
    /// A <see cref="LfsObjectError"/> wrapping <c>"HttpStatusCode.UnprocessableEntity"</c>.
    /// </summary>
    public static LfsObjectError UnprocessableEntity(string message = "Validation error")
        => new() { Code = HttpStatusCode.UnprocessableEntity, Message = message };

    /// <summary>
    /// A <see cref="LfsObjectError"/> representing an <see cref="Exception"/>.
    /// </summary>
    public static LfsObjectError FromException(Exception ex, HttpStatusCode code = HttpStatusCode.BadRequest)
        => new() { Code = code, Message = ex.Message };
}