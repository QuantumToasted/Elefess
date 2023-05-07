using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// Represents a Git LFS batch transfer request as it would be sent by a <c>git-lfs</c> client.
/// </summary>
/// <param name="Operation">The requested operation, <see cref="LfsOperation.Upload"/> or <see cref="LfsOperation.Download"/>.</param>
/// <param name="Objects">A collection of <see cref="LfsRequestObject"/>s representing the objects being uploaded or downloaded.</param>
/// <param name="Ref">A <see cref="LfsRequestObjectRef"/> object, used for more advanced or complicated authentication solutions.</param>
/// <param name="HashAlgorithm">The hash algorithm the objects' OIDs are computed with. Defaults to <c>SHA-256</c> if none is provided.</param>
public sealed record LfsBatchTransferRequest(
    [property: JsonPropertyName("operation"), JsonConverter(typeof(JsonStringEnumConverter))]
        LfsOperation Operation,
    [property: JsonPropertyName("objects")]
        ICollection<LfsRequestObject> Objects,
    [property: JsonPropertyName("ref"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        LfsRequestObjectRef? Ref = null,
    [property: JsonPropertyName("hash_algo"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        string? HashAlgorithm = LfsUtil.Constants.HashAlgorithms.SHA256)
{
    [JsonPropertyName("transfers"), JsonConverter(typeof(LfsTransferCollectionJsonConverter))]
    private readonly ICollection<LfsTransferAdapter>? _transferAdapters = null;

    /// <summary>
    /// A collection of <see cref="LfsTransferAdapter"/>s that the requesting client supports or is compatible with.
    /// </summary>
    [JsonIgnore]
    public ICollection<LfsTransferAdapter> RequestedTransferAdapters => _transferAdapters ?? new[] { LfsTransferAdapter.Basic };
}