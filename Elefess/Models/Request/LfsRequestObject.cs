using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// A Git LFS batch request object.
/// </summary>
/// <param name="Oid">The OID, or file hash, of the object.</param>
/// <param name="Size">The file size of the object.</param>
public sealed record LfsRequestObject(
    [property: JsonPropertyName("oid")]
        string Oid, 
    [property: JsonPropertyName("size")]
        long Size);