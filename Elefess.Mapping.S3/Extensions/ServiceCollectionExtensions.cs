using System;
using Microsoft.Extensions.DependencyInjection;

namespace Elefess.Mapping.S3;

/// <summary>
/// Various extension methods for registering an S3 OID mapper with an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers an <see cref="S3OidMapper"/> with the given configuration values, as well as an <see cref="ILfsOidMapper"/> which accesses the S3 mapper. 
        /// (See <see cref="S3OidMapperConfiguration"/>)
        /// </summary>
        /// <param name="bucketName">The bucket name to configure the mapper with.</param>
        /// <param name="expiryDuration">The request expiry duration to configure the mapper with.</param>
        /// <param name="contentType">The <c>Content-Type</c> header value to configure the mapper with.</param>
        /// <param name="allowOverwriting">Whether the mapper should be configured to allow overwrites on uploads.</param>
        /// <returns>The existing service collection with the mapper registered.</returns>
        public IServiceCollection AddS3OidMapper(string bucketName, TimeSpan? expiryDuration = null, string? contentType = null, bool allowOverwriting = false)
        {
            return services.AddS3OidMapper(new S3OidMapperConfiguration
            {
                BucketName = bucketName,
                ExpiryDuration = expiryDuration ?? S3OidMapperConfiguration.DefaultExpiryDuration,
                ContentType = contentType ?? S3OidMapperConfiguration.DEFAULT_CONTENT_TYPE,
                AllowOverwriting = allowOverwriting
            });
        }
        
        /// <summary>
        /// Registers an <see cref="S3OidMapper"/> with an optional configuration, as well as an <see cref="ILfsOidMapper"/> which accesses the S3 mapper.
        /// </summary>
        /// <param name="configuration">The configuration to configure the mapper with.</param>
        /// <remarks>If no configuration is provided, the service provider will expect an instance of <see cref="S3OidMapperConfiguration"/> to be registered.</remarks>
        /// <returns>The existing service collection with the mapper registered.</returns>
        public IServiceCollection AddS3OidMapper(S3OidMapperConfiguration? configuration = null)
        {
            if (configuration is not null)
                services.AddSingleton(configuration);
            
            return services.AddSingleton<S3OidMapper>()
                .AddSingleton<ILfsOidMapper>(static x => x.GetRequiredService<S3OidMapper>());
        }
    }
}