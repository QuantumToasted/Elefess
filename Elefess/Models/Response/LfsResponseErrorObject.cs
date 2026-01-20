using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// A Git LFS response object representing an error or problem that occurred while mapping the corresponding request object.
/// </summary>
public sealed class LfsResponseErrorObject : LfsResponseObject
{
    /// <summary>
    /// An <see cref="LfsObjectError"/> representing the problem that occurred for this object.
    /// </summary>
    [JsonPropertyName("error")]
    public required LfsObjectError Error { get; init; }
}