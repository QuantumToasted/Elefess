using Elefess.Models;

namespace Elefess;

/// <summary>
/// Represents a Git LFS OID mapper, responsible for mapping an input OID, size, and operation from a batch request to an appropriate LFS object. 
/// </summary>
public interface ILfsOidMapper
{
    /// <summary>
    /// Maps an OID and size, with the requested operation, to an LFS object.
    /// </summary>
    /// <param name="oid">The OID or file hash of the request object.</param>
    /// <param name="size">The file size of the request object, in bytes.</param>
    /// <param name="operation">The requested operation, either <see cref="LfsOperation.Upload"/> or <see cref="LfsOperation.Download"/>.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>A <see cref="Task"/> representing either an <see cref="LfsResponseDataObject"/> if successful, or an <see cref="LfsResponseErrorObject"/> if validation errors occur.</returns>
    Task<LfsResponseObject> MapObjectAsync(string oid, long size, LfsOperation operation, CancellationToken cancellationToken);
}