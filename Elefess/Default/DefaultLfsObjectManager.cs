using Elefess.Models;

namespace Elefess;

/// <summary>
/// A default Git LFS object manager which creates LFS objects by mapping them via a provided <see cref="ILfsOidMapper"/>.
/// </summary>
public sealed class DefaultLfsObjectManager : ILfsObjectManager
{
    private readonly ILfsOidMapper _mapper;

    /// <summary>
    ///  Creates an instance of a <see cref="DefaultLfsObjectManager"/> using a provided OID mapper.
    /// </summary>
    /// <param name="mapper">The OID mapper to use.</param>
    public DefaultLfsObjectManager(ILfsOidMapper mapper)
    {
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<LfsResponseObject>> CreateObjectsAsync(IList<LfsRequestObject> objects, LfsOperation operation, CancellationToken cancellationToken)
    {
        var responseObjectTasks = objects.Select(x => _mapper.MapObjectAsync(x.Oid, x.Size, operation, cancellationToken));
        var responseObjects = await Task.WhenAll(responseObjectTasks).ConfigureAwait(false);

        for (var i = 0; i < objects.Count; i++)
        {
            var responseObject = responseObjects[i];

            if (responseObject.HasData)
                continue;
            
            if (responseObject is LfsResponseDataObject dataObject)
            {
                responseObjects[i] = dataObject with { Oid = objects[i].Oid, Size = objects[i].Size, Authenticated = true };
            }
            else
            {
                responseObjects[i] = responseObject with { Oid = objects[i].Oid, Size = objects[i].Size };
            }
        }

        return responseObjects;
    }
}