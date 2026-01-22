using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// The main object returned with a Git LFS batch object response.
/// </summary>
public sealed class LfsBatchTransferResponse
{
    /// <summary>
    /// The transfer adapter selected by the <see cref="ILfsTransferSelector"/>.
    /// </summary>
    [JsonPropertyName("transfer"), JsonConverter(typeof(LfsTransferJsonConverter))]
    public required LfsTransferAdapter TransferAdapter { get; init; }

    /// <summary>
    /// A collection of <see cref="LfsResponseDataObject"/> and/or <see cref="LfsResponseErrorObject"/>s representing the response objects.
    /// </summary>
    [JsonPropertyName("hash_algo"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? HashAlgorithm { get; init; }
    
    /// <summary>
    /// The hash algorithm the response objects' OIDs use.
    /// </summary>
    [JsonPropertyName("objects")]
    public required IReadOnlyCollection<LfsResponseObject> Objects { get; init; }
}