using System.Net;
using System.Text.Json.Serialization;

namespace Elefess.Core.Models;

[JsonDerivedType(typeof(LfsResponseDataObject))]
[JsonDerivedType(typeof(LfsResponseErrorObject))]
public abstract record LfsResponseObject(
    [property: JsonPropertyName("oid"), JsonPropertyOrder(1)]
        string Oid,
    [property: JsonPropertyName("size"), JsonPropertyOrder(2)]
        long Size)
{
    public static LfsResponseErrorObject FromError(LfsObjectError error)
    {
        return new LfsResponseErrorObject(default!, default, error)
        {
            HasData = false
        };
    }

    public static LfsResponseErrorObject FromError(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new LfsResponseErrorObject(default!, default, new LfsObjectError(statusCode, message))
        {
            HasData = false
        };
    }
    
    public static LfsResponseDataObject BasicUpload(Uri uri, IReadOnlyDictionary<string, string>? headers = null, int? expiryInSeconds = null, DateTimeOffset? expiresAt = null)
    {
        return new LfsResponseDataObject(default!, default, new Dictionary<string, LfsResponseObjectAction>
        {
            [LfsUtil.Constants.Actions.UPLOAD] = new(uri, headers, expiryInSeconds, expiresAt)
        })
        {
            HasData = false
        };
    }
    
    public static LfsResponseDataObject BasicDownload(Uri uri, IReadOnlyDictionary<string, string>? headers = null, int? expiryInSeconds = null, DateTimeOffset? expiresAt = null)
    {
        return new LfsResponseDataObject(default!, default, new Dictionary<string, LfsResponseObjectAction>
        {
            [LfsUtil.Constants.Actions.DOWNLOAD] = new(uri, headers, expiryInSeconds, expiresAt)
        })
        {
            HasData = false
        };
    }

    [JsonIgnore] 
    internal bool HasData { get; private init; } = true;
}