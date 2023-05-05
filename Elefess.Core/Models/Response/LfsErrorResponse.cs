using System.Net;
using System.Text.Json.Serialization;

namespace Elefess.Core.Models;

public sealed record LfsErrorResponse(
    /*[property: JsonIgnore] 
        HttpStatusCode ResponseStatusCode,*/
    [property: JsonPropertyName("message")]
        string Message,
    [property: JsonPropertyName("documentation_url"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        Uri? DocumentationUri = null,
    [property: JsonPropertyName("request_id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        string? RequestId = null/*,
    /[property: JsonIgnore] 
        string? LfsAuthenticationHeaderValue = null*/);