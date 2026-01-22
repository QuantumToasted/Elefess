using Elefess.Models;

namespace Elefess.TestHost.AspNetCore;

public sealed class MockLfsOidMapper : ILfsOidMapper
{
    public const long VALID_SIZE = 159734862;
    
    public Task<LfsResponseObject> MapObjectAsync(string oid, long size, LfsOperation operation, CancellationToken cancellationToken)
    {
        LfsResponseObject response = operation switch
        {
            LfsOperation.Upload => LfsResponseObject.BasicUpload(new($"https://example.com/upload/{oid}")),
            LfsOperation.Download when size != VALID_SIZE => LfsResponseObject.FromError(LfsObjectError.UnprocessableEntity("Size mismatch")),
            LfsOperation.Download => LfsResponseObject.BasicDownload(new($"https://example.com/download/{oid}")),
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };

        return Task.FromResult(response);
    }
}