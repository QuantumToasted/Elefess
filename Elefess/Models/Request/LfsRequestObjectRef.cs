using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// A Git LFS <c>ref</c> request object reference.
/// </summary>
/// <remarks>
/// See the <a href="https://github.com/git-lfs/git-lfs/blob/main/docs/api/batch.md#ref-property">Git LFS documentation</a>
/// for more information on this property.
/// </remarks>
public sealed class LfsRequestObjectRef
{
    /// <summary>
    /// The name of the object reference.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}