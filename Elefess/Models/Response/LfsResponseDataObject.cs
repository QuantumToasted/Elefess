using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// A Git LFS response object representing a successful response, including additional object data such as actions.
/// </summary>
public sealed class LfsResponseDataObject : LfsResponseObject
{
    /// <summary>
    /// A mapping of action types (such as <c>upload</c> or <c>download</c>) to actions.
    /// </summary>
    [JsonPropertyName("actions")]
    public required IReadOnlyDictionary<string, LfsResponseObjectAction> Actions { get; init; }

    /// <summary>
    /// If anything but <see langword="true"/>, the URLs located in <see cref="Actions"/> require the same authentication this Git LFS server uses.
    /// </summary>
    [JsonPropertyName("authenticated")] 
    public bool? UsesGitLfsAuthentication { get; init; }
}