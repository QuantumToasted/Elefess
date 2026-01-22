using System.Security.Cryptography;
using System.Text;
using Amazon.Runtime;
using Amazon.S3;
using Elefess.Mapping.S3;
using Microsoft.Extensions.DependencyInjection;

namespace Elefess.Test;

public sealed class S3MapperWebApplicationFactory : MapperWebApplicationFactory<S3OidMapper>
{
    internal TemporaryFile File { get; set; } = TemporaryFile.Create();
    
    protected override void AddMapper(IServiceCollection services)
    {
        var accessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY")!;
        var secretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY")!;
        var baseUrl = Environment.GetEnvironmentVariable("S3_BASE_URL")!;
        var bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_ID")!;


        services
            .AddSingleton(new BasicAWSCredentials(accessKey, secretKey))
            .AddSingleton<AWSCredentials>(static x => x.GetRequiredService<BasicAWSCredentials>())
            .AddSingleton(new AmazonS3Config { ServiceURL = baseUrl })
            .AddSingleton<AmazonS3Client>()
            .AddSingleton<IAmazonS3>(static x => x.GetRequiredService<AmazonS3Client>())
            .AddS3OidMapper(bucketName);
    }
    
    internal sealed class TemporaryFile(byte[] data)
    {
        public MemoryStream Stream { get; } = new(data);

        public string Hash { get; } = ComputeHash(data);

        public static TemporaryFile Create(long size = 1597346)
        {
            var buffer = new byte[size];
            Random.Shared.NextBytes(buffer);
            return new TemporaryFile(buffer);
        }

        private static string ComputeHash(byte[] data)
        {
            var sb = new StringBuilder();

            var result = SHA256.HashData(data);
            sb.AppendJoin("", result.Select(x => x.ToString("x2")));

            return sb.ToString();
        }
    }
}