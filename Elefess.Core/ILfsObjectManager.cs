using Elefess.Core.Models;

namespace Elefess.Core;

public interface ILfsObjectManager
{
    Task<IReadOnlyCollection<LfsResponseObject>> CreateUploadObjectsAsync(IList<LfsRequestObject> objects, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<LfsResponseObject>> CreateDownloadObjectsAsync(IList<LfsRequestObject> objects, CancellationToken cancellationToken);
}