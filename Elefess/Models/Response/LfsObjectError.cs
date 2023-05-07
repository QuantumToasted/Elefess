using System.Net;
using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// The specific error model for a Git LFS response error object.
/// </summary>
/// <param name="Code">The status code indicating the type of problem that occurred.</param>
/// <param name="Message">A message with details about the error or problem that occurred.</param>
public sealed record LfsObjectError(
    [property: JsonPropertyName("code")]
    HttpStatusCode Code,
    [property: JsonPropertyName("message")]
        string Message)
{
    /// <summary>
    /// A <see cref="LfsObjectError"/> wrapping <see cref="HttpStatusCode.NotFound"/>.
    /// </summary>
    public static LfsObjectError NotFound(string message = "The object does not exist on the server")
        => new(HttpStatusCode.NotFound, message);

    /// <summary>
    /// A <see cref="LfsObjectError"/> wrapping <see cref="HttpStatusCode.Conflict"/>.
    /// </summary>
    public static LfsObjectError Conflict(string message = "The specified hash algorithm disagrees with the server's acceptable options")
        => new(HttpStatusCode.Conflict, message);

    /// <summary>
    /// A <see cref="LfsObjectError"/> wrapping <see cref="HttpStatusCode.Gone"/>.
    /// </summary>
    public static LfsObjectError Gone(string message = "The object was removed by the owner")
        => new(HttpStatusCode.Gone, message);

    /// <summary>
    /// A <see cref="LfsObjectError"/> wrapping <see cref="HttpStatusCode.UnprocessableEntity"/>.
    /// </summary>
    public static LfsObjectError UnprocessableEntity(string message = "Validation error")
        => new(HttpStatusCode.UnprocessableEntity, message);
    
    /// <summary>
    /// A <see cref="LfsObjectError"/> representing an <see cref="Exception"/>.
    /// </summary>
    public static LfsObjectError FromException(Exception ex, HttpStatusCode code = HttpStatusCode.BadRequest)
        => new(code, ex.Message);
}