# Elefess.Authenticators.GitHub
Elefess.Authenticators.GitHub is a custom `ILfsAuthenticator` implementation for Elefess which utilizes a GitHub username & personal access token for Basic authorization.

## Quick start
*See the main Elefess [quick start guide](../Elefess/README.md#quick-start) for core Elefess-related information.*

While registering your services:
```cs
// Registers an instance of GitHubLfsAuthenticator, an implementation of ILfsAuthenticator
services.AddGitHubAuthenticator("MyGitHubUsername", "MyGitHubRepository");
```
Git LFS clients should be expected to send a `Basic` Authorization header with the encoded value being your GitHub username and a [personal access token](https://github.com/settings/tokens/new) with permission to view the repository used for the authenticator.

An alternative method which does not require passing your organization/username and repository names directly can also be used, but this requires you to additionally register an instance of [GitHubLfsAuthenticatorOptions](GitHubLfsAuthenticatorOptions.cs) to your service provider: 
```cs
public sealed class GitHubLfsAuthenticatorOptions
{
    public string Organization { get; init; } = string.Empty;
    public string Repository { get; init; } = string.Empty;
    public Uri BaseAddress { get; init; } = new("https://api.github.com/");
}
```