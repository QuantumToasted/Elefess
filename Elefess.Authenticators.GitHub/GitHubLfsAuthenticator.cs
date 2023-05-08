using Elefess.Models;
using Octokit;

namespace Elefess.Authenticators.GitHub;

/// <summary>
/// A Git LFS authenticator which authenticates with GitHub using a user's username and personal access token.
/// </summary>
/// <remarks>The personal access token used in <see cref="AuthenticateAsync"/> should have the ability to read the repository.</remarks>
public sealed class GitHubLfsAuthenticator : ILfsAuthenticator
{
    private readonly GitHubLfsAuthenticatorOptions _options;

    /// <summary>
    /// Creates an instance of a <see cref="GitHubLfsAuthenticator"/> using a provided set of options.
    /// </summary>
    /// <param name="options">The options to configure this instance.</param>
    public GitHubLfsAuthenticator(GitHubLfsAuthenticatorOptions options)
    {
        _options = options;
    }

    /// <inheritdoc />
    public async Task AuthenticateAsync(string username, string authToken, LfsOperation operation, CancellationToken cancellationToken)
    {
        var client = CreateRepositoriesClient(_options.BaseAddress, username, authToken);

        var repository = await client.Get(_options.Organization, _options.Repository);

        var missingPermission = operation == LfsOperation.Download && !repository.Permissions.Pull
            ? "pull"
            : operation == LfsOperation.Upload && !repository.Permissions.Push
                ? "push"
                : null;

        if (missingPermission is not null)
        {
            throw new InvalidOperationException(
                $"User {username} lacks the required \"{missingPermission}\" permission for the repository {_options.Organization}/{_options.Repository}.");
        }
    }

    private static IRepositoriesClient CreateRepositoriesClient(Uri baseApiAddress, string username, string authToken)
    {
        var assemblyName = typeof(ILfsAuthenticator).Assembly.GetName();

        var headerValue = new ProductHeaderValue(assemblyName.Name, assemblyName.Version!.ToString(3));

        var client = new GitHubClient(new Connection(headerValue, baseApiAddress))
        {
            Credentials = new Credentials(username, authToken)
        };

        return client.Repository;
    }
}