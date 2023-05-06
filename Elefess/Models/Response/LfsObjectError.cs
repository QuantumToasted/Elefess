using System.Net;
using System.Text.Json.Serialization;

namespace Elefess.Models;

public sealed record LfsObjectError(
    [property: JsonPropertyName("code")] 
        HttpStatusCode Code,
    [property: JsonPropertyName("message")]
        string Message)
{
    public static LfsObjectError NotFound(string message = "The object does not exist on the server")
        => new(HttpStatusCode.NotFound, message);

    public static LfsObjectError Conflict(string message = "The specified hash algorithm disagrees with the server's acceptable options")
        => new(HttpStatusCode.Conflict, message);

    public static LfsObjectError Gone(string message = "The object was removed by the owner")
        => new(HttpStatusCode.Conflict, message);

    public static LfsObjectError UnprocessableEntity(string message = "Validation error")
        => new(HttpStatusCode.UnprocessableEntity, message);
    
    public static LfsObjectError FromException(Exception ex, HttpStatusCode code = HttpStatusCode.BadRequest)
        => new(code, ex.Message);
}