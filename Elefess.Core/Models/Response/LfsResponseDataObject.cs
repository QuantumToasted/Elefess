using System.Text.Json.Serialization;

namespace Elefess.Core.Models;

public sealed record LfsResponseDataObject(
        string Oid,
        long Size,
    [property: JsonPropertyName("actions"), JsonPropertyOrder(3)]
        IReadOnlyDictionary<string, LfsResponseObjectAction> Actions,
    [property: JsonPropertyName("authenticated"), JsonPropertyOrder(4), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        bool? Authenticated = null) : LfsResponseObject(Oid, Size);