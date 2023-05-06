using Elefess;
using Elefess.Models;

namespace Elefess.TestHost.AspNetCore;

public class DummyAuthenticator : ILfsAuthenticator
{
    public Task AuthenticateAsync(string id, string password, LfsOperation requestedOperation, CancellationToken cancellationToken)
    {
         return Task.CompletedTask;
    }
}