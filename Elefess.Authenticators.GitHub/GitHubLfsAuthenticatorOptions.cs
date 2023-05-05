namespace Elefess.Authenticators.GitHub;

public sealed class GitHubLfsAuthenticatorOptions
{
    public string Repository { get; set; } = string.Empty;

    public string Organization { get; set; } = string.Empty;

    public Uri BaseAddress { get; set; } = new("https://api.github.com/");
}