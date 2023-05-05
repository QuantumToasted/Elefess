using System.Text.Json.Serialization;

namespace Elefess.Core.Models;

public sealed record LfsResponseErrorObject(
        string Oid,
        long Size,
    [property: JsonPropertyName("error"), JsonPropertyOrder(3)] 
        LfsObjectError Error) : LfsResponseObject(Oid, Size);