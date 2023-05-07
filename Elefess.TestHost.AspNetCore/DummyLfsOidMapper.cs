using System.Net;
using Elefess.Models;

namespace Elefess.TestHost.AspNetCore;

public sealed class DummyLfsOidMapper : ILfsOidMapper
{
    public Task<LfsResponseObject> MapObjectAsync(string oid, long size, LfsOperation operation, CancellationToken cancellationToken)
    {
        try
        {
            switch (operation)
            {
                case LfsOperation.Upload:
                {
                    if (oid != Constants.VALID_UPLOAD_OID)
                        throw new KeyNotFoundException();
                
                    return Task.FromResult<LfsResponseObject>(LfsResponseObject.BasicUpload(new($"https://example.com/upload/{oid}")));
                }
                case LfsOperation.Download:
                {
                    if (oid != Constants.VALID_DOWNLOAD_OID)
                        throw new KeyNotFoundException();
            
                    return Task.FromResult<LfsResponseObject>(LfsResponseObject.BasicDownload(new($"https://example.com/download/{oid}")));
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }
        catch (KeyNotFoundException)
        {
            return Task.FromResult<LfsResponseObject>(LfsResponseObject.FromError($"OID lookup failed for {oid}", HttpStatusCode.NotFound));
        }
        catch (Exception ex)
        {
            return Task.FromResult<LfsResponseObject>(LfsResponseObject.FromError(ex.Message));
        }
    }
}