using System.Net;
using System.Text.Json.Serialization;

namespace Elefess.Models;

/// <summary>
/// A base Git LFS response object.
/// </summary>
[JsonDerivedType(typeof(LfsResponseErrorObject), "error")]
[JsonDerivedType(typeof(LfsResponseDataObject), "actions")]
public abstract class LfsResponseObject
{
    internal string _oid = null!;
    internal long _size;
    
    /// <summary>
    /// The OID, or file hash, of the response object.
    /// </summary>
    [JsonPropertyName("oid")]
    public required string Oid { get => _oid; init => _oid = value; }
    
    /// <summary>
    /// The file size of the response object
    /// </summary>
    [JsonPropertyName("size")]
    public required long Size { get => _size; init => _size = value; }
    
    [JsonIgnore] 
    internal bool HasData { get; private init; } = true;
    
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
        return new LfsResponseErrorObject
        {
            Oid = null!,
            Size = 0,
            Error = error,
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
        return new LfsResponseErrorObject
        {
            Oid = null!,
            Size = 0,
            Error = new LfsObjectError { Code = statusCode, Message = message },
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
    public static LfsResponseDataObject BasicUpload(Uri uri, IReadOnlyDictionary<string, string>? headers = null, DateTimeOffset? expiresAt = null,
        bool? useGitLfsAuthentication = false)
    {
        return new LfsResponseDataObject
        {
            Oid = null!,
            Size = 0,
            Actions = new Dictionary<string, LfsResponseObjectAction>
            {
                [LfsUtil.Constants.Actions.UPLOAD] = new() { Uri = uri, Headers = headers, ExpiresAt = expiresAt }
            },
            UsesGitLfsAuthentication = useGitLfsAuthentication,
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
    public static LfsResponseDataObject BasicDownload(Uri uri, IReadOnlyDictionary<string, string>? headers = null, DateTimeOffset? expiresAt = null,
        bool? useGitLfsAuthentication = false)
    {
        return new LfsResponseDataObject
        {
            Oid = null!,
            Size = 0,
            Actions = new Dictionary<string, LfsResponseObjectAction>
            {
                [LfsUtil.Constants.Actions.DOWNLOAD] = new() { Uri = uri, Headers = headers, ExpiresAt = expiresAt }
            },
            UsesGitLfsAuthentication = useGitLfsAuthentication,
            HasData = false
        };
    }
}