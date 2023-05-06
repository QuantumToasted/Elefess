using System.Text.Json.Serialization;

namespace Elefess.Models;

public sealed record LfsBatchTransferResponse(
    [property: JsonPropertyName("transfer"), JsonConverter(typeof(LfsTransferJsonConverter)), JsonPropertyOrder(1)]
        LfsTransfer Transfer,
    [property: JsonPropertyName("objects"), JsonConverter(typeof(LfsResponseObjectCollectionJsonConverter)), JsonPropertyOrder(3)]
        IReadOnlyCollection<LfsResponseObject> Objects,
    [property: JsonPropertyName("hash_algo"), JsonPropertyOrder(2), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        string? HashAlgorithm = null);