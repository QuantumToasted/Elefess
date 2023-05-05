using Elefess.Core.Models;

namespace Elefess.Core;

public sealed class DictionaryLfsAuthenticator : ILfsAuthenticator
{
    private readonly IReadOnlyDictionary<string, string> _credentials;

    public DictionaryLfsAuthenticator(IDictionary<string, string> credentials)
    {
        _credentials = credentials.AsReadOnly();
    }

    public Task AuthenticateAsync(string id, string password, LfsOperation requestedOperation, CancellationToken cancellationToken)
    {
        if (!_credentials.TryGetValue(id, out var p) || p != password)
            throw new InvalidOperationException("Provided username/password combination is invalid.");

        return Task.FromResult<string?>(null);
    }
}