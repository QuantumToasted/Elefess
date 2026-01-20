using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// Represents a Git LFS batch transfer request as it would be sent by a <c>git-lfs</c> client.
/// </summary>
public sealed class LfsBatchTransferRequest
{
    /// <summary>
    /// The requested operation, <see cref="LfsOperation.Upload"/> or <see cref="LfsOperation.Download"/>.
    /// </summary>
    [JsonPropertyName("operation"), JsonConverter(typeof(JsonStringEnumConverter))]
    public required LfsOperation Operation { get; init; }
    
    /// <summary>
    /// A collection of <see cref="LfsRequestObject"/>s representing the objects being uploaded or downloaded.
    /// </summary>
    [JsonPropertyName("objects")]
    public required ICollection<LfsRequestObject> Objects { get; init; }

    /// <summary>
    /// A <see cref="LfsRequestObjectRef"/> object, used for more advanced or complicated authentication solutions.
    /// </summary>
    [JsonPropertyName("ref"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LfsRequestObjectRef? Ref { get; init; }

    /// <summary>
    /// The hash algorithm the objects' OIDs are computed with.
    /// </summary>
    /// <remarks>Defaults to <c>SHA-256</c> if none is provided.</remarks>
    [JsonPropertyName("hash_algo"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? HashAlgorithm { get; init; } = LfsUtil.Constants.HashAlgorithms.SHA256;

    /// <summary>
    /// A collection of <see cref="LfsTransferAdapter"/>s that the requesting client supports or is compatible with.
    /// </summary>
    /// <remarks>Defaults to [<see cref="LfsTransferAdapter.Basic"/>] if none are provided.</remarks>
    [JsonPropertyName("transfers"), JsonConverter(typeof(LfsTransferCollectionJsonConverter))]
    public ICollection<LfsTransferAdapter> TransferAdapters { get; init; } = [LfsTransferAdapter.Basic];
}