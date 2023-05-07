namespace Elefess.Authenticators.GitHub;
/// <summary>
/// A collection of configuration properties for a <see cref="GitHubLfsAuthenticator"/>.
/// </summary>
public sealed class GitHubLfsAuthenticatorOptions
{
    /// <summary>
    /// The organization (or user) that <see cref="Repository"/> belongs to.
    /// </summary>
    public string Organization { get; init; } = string.Empty;
    
    /// <summary>
    /// The repository name to authenticate against.
    /// </summary>
    public string Repository { get; init; } = string.Empty;

    /// <summary>
    /// The base API route when making authentication requests. Defaults to https://api.github.com/.
    /// </summary>
    public Uri BaseAddress { get; init; } = new("https://api.github.com/");
}