using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// A Git LFS response object representing a successful response, including additional object data such as actions.
/// </summary>
/// <param name="Oid">The OID, or file hash, of the response object.</param>
/// <param name="Size">The file size of the response object.</param>
/// <param name="Actions">A mapping of action types (such as <c>upload</c> or <c>download</c>) to actions.</param>
/// <param name="Authenticated">If anything but <see langword="true"/>, the URLs located in <see cref="Actions"/> require the same authentication this Git LFS server uses.</param>
public sealed record LfsResponseDataObject(
        string Oid,
        long Size,
    [property: JsonPropertyName("actions"), JsonPropertyOrder(3)]
        IReadOnlyDictionary<string, LfsResponseObjectAction> Actions,
    [property: JsonPropertyName("authenticated"), JsonPropertyOrder(4), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        bool? Authenticated = null) : LfsResponseObject(Oid, Size);