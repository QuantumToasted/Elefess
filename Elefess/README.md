# Elefess
Elefess is a Git LFS server implementation following the [API spec](https://github.com/git-lfs/git-lfs/blob/8e96b5d5d84095d1c0dd0c550d8fdf2c8c5c6456/docs/api/README.md), allowing for the design and usage of fully custom Git LFS servers beyond those provided by popular Git hosts such as GitHub or GitLab.

## Quick start
Elefess on its own **does not** function as a Git LFS server out of the box. You will need a hosting setup such as [Elefess.Hosting.AspNetCore](../Elefess.Hosting.AspNetCore/README.md) or your own hosting solution to run a server. This project contains the core interfaces (+ a few implementations) and the API JSON models.

## Elefess core types
- All official Elefess hosting implementation types will access and execute its appropriate types and methods in the following order - Custom or third-party implementations are certainly not required or expected to follow it strictly, but it is recommended to do so for consistency.
- Elefess utilizes an `Exception`-based approach for its request execution pipeline to avoid boilerplate code or heavy reliance on third-party libraries. In lieu of a `Result`-based approach, implementations of Elefess' various interface types should be expected to throw the appropriate `Exception` in an error state, instead of returning an error value or string directly.
- All types which are required in the execution pipeline in first-party hosting solutions will be suffixed with an asterisk (*).

### 1. [ILfsAuthenticator](ILfsAuthenticator.cs) *
The Git LFS API uses [HTTP Basic Authentication](https://github.com/git-lfs/git-lfs/blob/8e96b5d5d84095d1c0dd0c550d8fdf2c8c5c6456/docs/api/authentication.md) to authorize requests. First in the request pipeline, an instance of `ILfsAuthenticator` is designed to authenticate a request from the (base64) decoded credentials supplied:
```cs
Task AuthenticateAsync(string id, string password, LfsOperation operation, CancellationToken cancellationToken);
```
In an example request with the `Authorization` header value of `Basic ZWxlZmVzczpzZWNyZXQx`, Elefess will decode and split this into an `id` of `elefess`, and a `password` of `secret1`, which will be passed into the `AuthenticateAsync` method above.

Implementations can utilize the `operation` parameter to determine, for example, whether the requesting client has appropriate read or write access to the file they are trying to upload or download.

Current Elefess implementations include support for [GitHub](../Elefess.Authenticators.GitHub/README.md), with more planned for the future.

### 2. [ILfsRequestValidator](ILfsRequestValidator.cs)
While not strictly a part of the Git LFS API spec, a server may find it desirable or necessary to arbitrarily validate and deny requests depending on what data is provided in the request body:
```cs
Task ValidateAsync(LfsBatchTransferRequest request);
```
Registering your own instance of `ILfsRequestValidator` is **not** required for any first-party hosting solutions, but if one is created and registered where appropriate, it will be executed.

### 3. [ILfsTransferSelector](ILfsTransferSelector.cs) *
In the event that a client and server support different, custom, or newer [Batch API transfer adapters](https://github.com/git-lfs/git-lfs/blob/8e96b5d5d84095d1c0dd0c550d8fdf2c8c5c6456/docs/api/README.md#batch-api) other than the `basic` transfer adapter, an implementation of `ILfsTransferSelector` can be used for this purpose:
```cs
Task<LfsTransferAdapter> SelectTransferAsync(ICollection<LfsTransferAdapter> requestedTransferAdapters, CancellationToken cancellationToken);
```
A request can include an array of transfer adapter names - or if none are specified, the server should assume the client supports and is asking for the `basic` transfer adapter. A default implementation, [BasicLfsTransferRequestSelector](Default/BasicLfsTransferRequestSelector.cs), which will automatically select the `basic` transfer adapter so long as the client supports it, is provided with Elefess.

### 4. [ILfsObjectManager](ILfsObjectManager.cs) * and [ILfsOidMapper](ILfsOidMapper.cs)
Once a request has been authenticated, and the request has been validated (when appropriate), an Elefess server will then convert request objects to response objects via an implementation of `ILfsObjectManager`:
```cs
Task<IReadOnlyCollection<LfsResponseObject>> CreateObjectsAsync(IList<LfsRequestObject> objects, LfsOperation operation, CancellationToken cancellationToken);
```
A default implementation, [DefaultLfsObjectManager](Default/DefaultLfsObjectManager.cs), is provided with Elefess, and simply converts request objects into response objects by passing each object into an implementation of `ILfsOidMapper`:
```cs
Task<LfsResponseObject> MapObjectAsync(string oid, long size, LfsOperation operation, CancellationToken cancellationToken);
```
An Elefess server implementing this interface would then be able to convert into an appropriate response object depending on the input data. For example:
- If a request is to upload OID `a0dca55d00d4e444d87326146a721ce41d494783ea9a0fdf7e2826a32a8bbc24` with size `374252189`, the server would return `LfsResponseObject.BasicUpload(new($"https://example.com/upload/a0dca55d00d4e444d87326146a721ce41d494783ea9a0fdf7e2826a32a8bbc24"))`, which would be converted to an `upload` action to POST to the `Uri` previously mentioned.
- If a request is to download OID `b6ea4353b49b583f501ea87778f7699e372a8b1c83fb86cd468b3c8b5d229ffb` with size `476885649`, if there is no file corresponding to that OID, the server would return `LfsResponseObject.FromError(LfsObjectError.NotFound())`, which would be converted to an `error` object indicating that the specified object could not be found.

<small>* *If the `DefaultLfsObjectManager` is utilized, an implementation of `ILfsOidMapper` **is** required. In the case of a different implementation being used, it is not necessarily required but can still be utilized in the manner detailed above.*</small>

