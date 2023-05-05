using Elefess.Core.Models;

namespace Elefess.Core;

public interface ILfsOidMapper
{
    Task<LfsResponseObject> MapUploadObjectAsync(string oid, long size, CancellationToken cancellationToken);
    Task<LfsResponseObject> MapDownloadObjectAsync(string oid, CancellationToken cancellationToken);
}