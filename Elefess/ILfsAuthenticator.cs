using Elefess.Models;

namespace Elefess;

public interface ILfsAuthenticator
{
    Task AuthenticateAsync(string id, string password, LfsOperation requestedOperation, CancellationToken cancellationToken); 
}