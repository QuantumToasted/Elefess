using Elefess.Models;

namespace Elefess;

/// <summary>
/// Represents a Git LFS object manager, responsible for creating response objects from their appropriate request objects in a batch request.
/// </summary>
public interface ILfsObjectManager
{
    /// <summary>
    /// Creates a collection of <see cref="LfsResponseObject"/>s based on their respective <see cref="LfsRequestObject"/>s.
    /// </summary>
    /// <param name="objects">A list of <see cref="LfsRequestObject"/>s supplied in the batch request.</param>
    /// <param name="operation">The requested operation, either <see cref="LfsOperation.Upload"/> or <see cref="LfsOperation.Download"/>.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>A <see cref="Task"/> representing the collection of <see cref="LfsResponseDataObject"/>s or <see cref="LfsResponseErrorObject"/>s that were created.</returns>
    /// <remarks>
    /// This method should only throw an <see cref="Exception"/> if all objects fail validation when <c>operation</c> is <see cref="LfsOperation.Upload"/>.
    /// Prefer to return <see cref="LfsResponseErrorObject"/>s instead where appropriate.
    /// </remarks>
    Task<IReadOnlyCollection<LfsResponseObject>> CreateObjectsAsync(IList<LfsRequestObject> objects, LfsOperation operation, CancellationToken cancellationToken);
}