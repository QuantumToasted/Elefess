using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// A Git LFS response object representing an error or problem that occurred while mapping the corresponding request object.
/// </summary>
/// <param name="Oid">The OID, or file hash, of the response object.</param>
/// <param name="Size">The file size of the response object.</param>
/// <param name="Error">An <see cref="LfsObjectError"/> representing the problem that occurred for this object.</param>
public sealed record LfsResponseErrorObject(
        string Oid,
        long Size,
    [property: JsonPropertyName("error"), JsonPropertyOrder(3)] 
        LfsObjectError Error) : LfsResponseObject(Oid, Size);