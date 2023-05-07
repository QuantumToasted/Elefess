using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// The main object returned with a Git LFS batch object response.
/// </summary>
/// <param name="TransferAdapter">The transfer adapter selected by the <see cref="ILfsTransferSelector"/>.</param>
/// <param name="Objects">A collection of <see cref="LfsResponseDataObject"/> and/or <see cref="LfsResponseErrorObject"/>s representing the response objects </param>
/// <param name="HashAlgorithm">The hash algorithm the response objects' OIDs use.</param>
public sealed record LfsBatchTransferResponse(
    [property: JsonPropertyName("transfer"), JsonConverter(typeof(LfsTransferJsonConverter)), JsonPropertyOrder(1)]
        LfsTransferAdapter TransferAdapter,
    [property: JsonPropertyName("objects"), JsonConverter(typeof(LfsResponseObjectCollectionJsonConverter)), JsonPropertyOrder(3)]
        IReadOnlyCollection<LfsResponseObject> Objects,
    [property: JsonPropertyName("hash_algo"), JsonPropertyOrder(2), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        string? HashAlgorithm = null);