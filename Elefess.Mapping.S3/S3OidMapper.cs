using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Elefess.Models;

namespace Elefess.Mapping.S3;

/// <summary>
/// Represents an <see cref="ILfsOidMapper"/> that uses an Amazon S3-compatible API to map OIDs.
/// </summary>
/// <param name="s3">The Amazon S3 client used for mapping OIDs.</param>
/// <param name="configuration">The configuration for this mapper.</param>
public sealed class S3OidMapper(IAmazonS3 s3, S3OidMapperConfiguration configuration) : ILfsOidMapper
{
    /// <summary>
    /// The Amazon S3 client provided to the mapper.
    /// </summary>
    public IAmazonS3 S3 { get; } = s3;

    /// <inheritdoc cref="S3OidMapperConfiguration.BucketName"/> 
    public string BucketName { get; } = configuration.BucketName;

    /// <inheritdoc cref="S3OidMapperConfiguration.ExpiryDuration"/> 
    public TimeSpan ExpiryDuration { get; } = configuration.ExpiryDuration;

    /// <inheritdoc cref="S3OidMapperConfiguration.AllowOverwriting"/> 
    public bool AllowOverwriting { get; } = configuration.AllowOverwriting;

    /// <inheritdoc cref="S3OidMapperConfiguration.ContentType"/> 
    public string ContentType { get; } = configuration.ContentType;

    /// <inheritdoc />
    public Task<LfsResponseObject> MapObjectAsync(string oid, long size, LfsOperation operation, CancellationToken cancellationToken)
    {
        return operation switch
        {
            LfsOperation.Upload => UploadAsync(oid, size, cancellationToken),
            LfsOperation.Download => DownloadAsync(oid, size, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };
    }
        
    private async Task<LfsResponseObject> UploadAsync(string oid, long size, CancellationToken cancellationToken)
    {
        try
        {
            var obj = await S3.GetObjectMetadataAsync(BucketName, oid, cancellationToken);

            if (obj.HttpStatusCode == HttpStatusCode.OK && !AllowOverwriting)
                return LfsResponseObject.FromError(LfsObjectError.UnprocessableEntity("Overwriting this object is not permitted"));
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        { }
        catch (AmazonS3Exception ex) when (ex.StatusCode != HttpStatusCode.OK)
        {
            return LfsResponseObject.FromError(new LfsObjectError { Code = ex.StatusCode, Message = ex.ErrorCode });
        }

        try
        {
            var expiresAt = DateTime.UtcNow.Add(ExpiryDuration);
            var req = new GetPreSignedUrlRequest
            {
                Key = oid,
                BucketName = BucketName,
                ContentType = ContentType,
                Expires = expiresAt,
                Headers = { ["Content-Length"] = $"{size}" },
                Verb = HttpVerb.PUT
            };

            var url = await S3.GetPreSignedURLAsync(req);

            return LfsResponseObject.BasicUpload(new Uri(url), headers: new Dictionary<string, string>
            {
                ["Content-Type"] = ContentType
            }, expiresAt: expiresAt);
        }
        catch (Exception ex)
        {
            return LfsResponseObject.FromError(LfsObjectError.FromException(ex));
        }
    }

    private async Task<LfsResponseObject> DownloadAsync(string oid, long size, CancellationToken cancellationToken)
    {
        GetObjectMetadataResponse obj;

        try
        {
            obj = await S3.GetObjectMetadataAsync(BucketName, oid, cancellationToken);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return LfsResponseObject.FromError(LfsObjectError.NotFound());
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode != HttpStatusCode.OK)
        {
            return LfsResponseObject.FromError(new LfsObjectError { Code = ex.StatusCode, Message = ex.ErrorCode });
        }

        if (obj.ContentLength != size)
            return LfsResponseObject.FromError(LfsObjectError.UnprocessableEntity("Size mismatch"));

        try
        {
            var expiresAt = DateTime.UtcNow.Add(ExpiryDuration);
            var url = await S3.GetPreSignedURLAsync(new GetPreSignedUrlRequest
            {
                BucketName = BucketName,
                Key = oid,
                Expires = expiresAt,
                Verb = HttpVerb.GET
            });

            return LfsResponseObject.BasicDownload(new Uri(url), expiresAt: expiresAt);
        }
        catch (Exception ex)
        {
            return LfsResponseObject.FromError(LfsObjectError.FromException(ex));
        }
    }
}