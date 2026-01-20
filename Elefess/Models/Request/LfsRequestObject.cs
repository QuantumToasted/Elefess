using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// A Git LFS batch request object.
/// </summary>
public sealed class LfsRequestObject
{
    /// <summary>
    /// The OID, or file hash, of the object.
    /// </summary>
    public required string Oid { get; init; }
    
    /// <summary>
    /// The file size of the object.
    /// </summary>
    public required long Size { get; init; }
}