using Elefess.Models;

namespace Elefess.TestHost.AspNetCore;

public sealed class DummyAuthenticator : ILfsAuthenticator
{
    public Task AuthenticateAsync(string id, string password, LfsOperation operation, CancellationToken cancellationToken)
    {
         return Task.CompletedTask;
    }
}