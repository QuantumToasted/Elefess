using Elefess.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Elefess.Authenticators.GitHub;

public static class ServiceCollectionExtensions
{
    // For power users with their own GitHubLfsAuthenticatorOptions
    public static IServiceCollection AddGitHubAuthenticator(this IServiceCollection services)
    {
        services.AddSingleton<GitHubLfsAuthenticator>();
        services.AddSingleton<ILfsAuthenticator>(static x => x.GetRequiredService<GitHubLfsAuthenticator>());

        return services;
    }
    
    public static IServiceCollection AddGitHubAuthenticator(this IServiceCollection services,
        string organization, string repository, string baseAddress = "https://api.github.com/")
    {
        services.AddSingleton(new GitHubLfsAuthenticatorOptions
        {
            Organization = organization,
            Repository = repository,
            BaseAddress = new(baseAddress)
        });
        
        services.AddSingleton<GitHubLfsAuthenticator>();
        services.AddSingleton<ILfsAuthenticator>(static x => x.GetRequiredService<GitHubLfsAuthenticator>());

        return services;
    }
}