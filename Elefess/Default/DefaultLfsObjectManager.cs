using Elefess.Models;

namespace Elefess;

public sealed class DefaultLfsObjectManager : ILfsObjectManager
{
    private readonly ILfsOidMapper _mapper;

    public DefaultLfsObjectManager(ILfsOidMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<LfsResponseObject>> CreateUploadObjectsAsync(IList<LfsRequestObject> objects, CancellationToken cancellationToken)
    {
        var uploadTasks = objects.Select(x => _mapper.MapUploadObjectAsync(x.Oid, x.Size, cancellationToken));
        var responseObjects = await Task.WhenAll(uploadTasks).ConfigureAwait(false);
        
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

    public async Task<IReadOnlyCollection<LfsResponseObject>> CreateDownloadObjectsAsync(IList<LfsRequestObject> objects, CancellationToken cancellationToken)
    {
        var downloadTasks = objects.Select(x => _mapper.MapDownloadObjectAsync(x.Oid, cancellationToken));
        var responseObjects = await Task.WhenAll(downloadTasks).ConfigureAwait(false);
        
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