# Elefess.Mapping.S3
Elefess.Mapping.S3 is a custom `ILfsOidMapper` implementation for Elefess which utilizes an Amazon S3 compatible API for mapping OIDs and storing files.

## Quick start
*See the main Elefess [quick start guide](../Elefess/README.md#quick-start) for core Elefess-related information.*

A configured `IAmazonS3` client (from `AWSSDK.S3`) is expected to be registered to your service provider.

While registering your services:
```cs
// Registers an instance of S3OidMapper, an implementation of ILfsOidMapper
services.AddS3OidMapper("elefess");
```

Alternatively, you may also register an instance of [S3OidMapperConfiguration](S3OidMapperConfiguration.cs) to your service provider.
```cs
public sealed class S3OidMapperConfiguration
{
    public required string BucketName { get; init; }
    public TimeSpan ExpiryDuration { get; init; } = TimeSpan.FromMinutes(1);
    public string ContentType { get; init; } = "application/octet-stream";
    public bool AllowOverwriting { get; init; } = false;
}
```