using System.Net;
using Elefess.Core;
using Elefess.Core.Models;
using Elefess.TestHost.AspNetCore;

namespace Elefess;

public sealed class DummyLfsOidMapper : ILfsOidMapper
{
    public Task<LfsResponseObject> MapUploadObjectAsync(string oid, long size, CancellationToken cancellationToken)
    {
        try
        {
            if (oid != Constants.VALID_UPLOAD_OID)
                throw new KeyNotFoundException();
            
            return Task.FromResult<LfsResponseObject>(LfsResponseObject.BasicUpload(new($"https://example.com/upload/{oid}")));
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

    public Task<LfsResponseObject> MapDownloadObjectAsync(string oid, CancellationToken cancellationToken)
    {
        try
        {
            if (oid != Constants.VALID_DOWNLOAD_OID)
                throw new KeyNotFoundException();
            
            return Task.FromResult<LfsResponseObject>(LfsResponseObject.BasicDownload(new($"https://example.com/download/{oid}")));
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