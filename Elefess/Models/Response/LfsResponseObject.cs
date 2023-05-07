using System.Net;
using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// A base Git LFS response object.
/// </summary>
/// <param name="Oid">The OID, or file hash, of the response object.</param>
/// <param name="Size">The file size of the response object.</param>
[JsonDerivedType(typeof(LfsResponseDataObject))]
[JsonDerivedType(typeof(LfsResponseErrorObject))]
public abstract record LfsResponseObject(
    [property: JsonPropertyName("oid"), JsonPropertyOrder(1)]
        string Oid,
    [property: JsonPropertyName("size"), JsonPropertyOrder(2)]
        long Size)
{
    /// <summary>
    /// An error response object which wraps an <see cref="LfsObjectError"/>.
    /// </summary>
    /// <remarks>
    /// This convenience method does not set <see cref="Oid"/> or <see cref="Size"/>.
    /// The <see cref="DefaultLfsObjectManager"/> sets these properties automatically, but this behavior may not be desired.
    /// <p>To avoid this behavior, do not use these convenience methods.</p>
    /// </remarks>
    public static LfsResponseErrorObject FromError(LfsObjectError error)
    {
        return new LfsResponseErrorObject(default!, default, error)
        {
            HasData = false
        };
    }

    /// <summary>
    /// An error response object created from an error message and status code.
    /// </summary>
    /// <remarks>
    /// This convenience method does not set <see cref="Oid"/> or <see cref="Size"/>.
    /// The <see cref="DefaultLfsObjectManager"/> sets these properties automatically, but this behavior may not be desired.
    /// <p>To avoid this behavior, do not use these convenience methods.</p>
    /// </remarks>
    public static LfsResponseErrorObject FromError(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new LfsResponseErrorObject(default!, default, new LfsObjectError(statusCode, message))
        {
            HasData = false
        };
    }
    
    /// <summary>
    /// A data response object representing a typical <c>basic</c> upload action.
    /// </summary>
    /// <remarks>
    /// This convenience method does not set <see cref="Oid"/> or <see cref="Size"/>.
    /// The <see cref="DefaultLfsObjectManager"/> sets these properties automatically, but this behavior may not be desired.
    /// <p>To avoid this behavior, do not use these convenience methods.</p>
    /// </remarks>
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
    
    /// <summary>
    /// A data response object representing a typical <c>basic</c> download action.
    /// </summary>
    /// <remarks>
    /// This convenience method does not set <see cref="Oid"/> or <see cref="Size"/>.
    /// The <see cref="DefaultLfsObjectManager"/> sets these properties automatically, but this behavior may not be desired.
    /// <p>To avoid this behavior, do not use these convenience methods.</p>
    /// </remarks>
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