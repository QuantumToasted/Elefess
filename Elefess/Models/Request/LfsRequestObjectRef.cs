using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// A Git LFS <c>ref</c> request object.
/// </summary>
/// <param name="Name"></param>
/// <remarks>See https://github.com/git-lfs/git-lfs/blob/main/docs/api/batch.md#ref-property for more information on this property.</remarks>
public sealed record LfsRequestObjectRef(
    [property: JsonPropertyName("name")] 
        string Name);