using Elefess.Models;

namespace Elefess;

/// <summary>
/// A Git LFS authenticator
/// </summary>
public sealed class DictionaryLfsAuthenticator : ILfsAuthenticator
{
    private readonly Dictionary<string, string> _credentials;

    /// <summary>
    /// Constructs a <see cref="DictionaryLfsAuthenticator"/> with existing credentials.
    /// </summary>
    /// <param name="credentials">The credentials that will be validated against.</param>
    public DictionaryLfsAuthenticator(IDictionary<string, string> credentials)
    {
        _credentials = credentials.ToDictionary(x => x.Key, x => x.Value);
    }

    /// <inheritdoc />
    public Task AuthenticateAsync(string id, string password, LfsOperation operation, CancellationToken cancellationToken)
    {
        if (!_credentials.TryGetValue(id, out var p) || p != password)
            throw new InvalidOperationException("Provided username/password combination is invalid.");

        return Task.CompletedTask;
    }
}