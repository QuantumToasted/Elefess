using Elefess.Models;

namespace Elefess;

public interface ILfsObjectManager
{
    Task<IReadOnlyCollection<LfsResponseObject>> CreateUploadObjectsAsync(IList<LfsRequestObject> objects, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<LfsResponseObject>> CreateDownloadObjectsAsync(IList<LfsRequestObject> objects, CancellationToken cancellationToken);
}