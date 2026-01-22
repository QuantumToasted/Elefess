using System;

namespace Elefess.Mapping.S3;

/// <summary>
/// Represents the configuration for an <see cref="S3OidMapper"/>.
/// </summary>
public sealed class S3OidMapperConfiguration
{
    /// <summary>
    /// The default <c>Content-Type</c> header.
    /// </summary>
    public const string DEFAULT_CONTENT_TYPE = "application/octet-stream";
    
    /// <summary>
    /// The default upload/download request expiry duration.
    /// </summary>
    public static readonly TimeSpan DefaultExpiryDuration = TimeSpan.FromMinutes(1);
    
    /// <summary>
    /// The bucket name to use for uploads and downloads.
    /// </summary>
    public required string BucketName { get; init; }
    
    /// <summary>
    /// The duration after which the returned upload/download request URLs should expire. Defaults to <see cref="DefaultExpiryDuration"/>.
    /// </summary>
    public TimeSpan ExpiryDuration { get; init; } = DefaultExpiryDuration;

    /// <summary>
    /// The <c>Content-Type</c> header value to expect when making requests. Defaults to <see cref="DEFAULT_CONTENT_TYPE"/>.
    /// </summary>
    public string ContentType { get; init; } = DEFAULT_CONTENT_TYPE;

    /// <summary>
    /// Whether the given <see cref="S3OidMapper"/> should allow overwriting existing files on upload.
    /// </summary>
    public bool AllowOverwriting { get; init; }
}