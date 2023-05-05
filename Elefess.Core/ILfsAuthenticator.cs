using Elefess.Core.Models;

namespace Elefess.Core;

public interface ILfsAuthenticator
{
    Task AuthenticateAsync(string id, string password, LfsOperation requestedOperation, CancellationToken cancellationToken); 
}