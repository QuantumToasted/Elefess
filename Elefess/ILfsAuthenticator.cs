using Elefess.Models;

namespace Elefess;

/// <summary>
/// Represents a Git LFS authenticator, responsible for validating <c>Basic</c> credentials supplied in a batch request.
/// </summary>
public interface ILfsAuthenticator
{
    /// <summary>
    /// Authenticates a user with the supplied ID and password decoded from a <c>Basic</c> Authorization header.
    /// </summary>
    /// <param name="id">The ID or username decoded from the <c>Basic</c> credentials.</param>
    /// <param name="password">The password decoded from the <c>Basic</c> credentials</param>
    /// <param name="operation">The requested operation, either <see cref="LfsOperation.Upload"/> or <see cref="LfsOperation.Download"/>.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>A <see cref="Task"/> representing the authentication work performed.</returns>
    /// <remarks>This method should throw an <see cref="Exception"/> if authentication fails.</remarks>
    Task AuthenticateAsync(string id, string password, LfsOperation operation, CancellationToken cancellationToken); 
}