using System.Text.Json.Serialization;

namespace Elefess.Models;

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
    private readonly ICollection<LfsTransfer>? _transferTypes = null;

    [JsonIgnore]
    public ICollection<LfsTransfer> RequestedTransferTypes => _transferTypes ?? new[] { LfsTransfer.Basic };
}