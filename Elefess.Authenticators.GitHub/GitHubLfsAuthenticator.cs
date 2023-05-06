using Elefess;
using Elefess.Models;
using Octokit;

namespace Elefess.Authenticators.GitHub;

public sealed class GitHubLfsAuthenticator : ILfsAuthenticator
{
    private readonly GitHubLfsAuthenticatorOptions _options;

    public GitHubLfsAuthenticator(GitHubLfsAuthenticatorOptions options)
    {
        _options = options;
    }

    public async Task AuthenticateAsync(string username, string authToken, LfsOperation requestedOperation, CancellationToken cancellationToken)
    {
        var client = CreateRepositoriesClient(_options.BaseAddress, username, authToken);

        var repository = await client.Get(_options.Organization, _options.Repository);

        var missingPermission = requestedOperation == LfsOperation.Download && !repository.Permissions.Pull
            ? "pull"
            : requestedOperation == LfsOperation.Upload && !repository.Permissions.Push
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