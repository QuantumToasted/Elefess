﻿using Microsoft.Extensions.DependencyInjection;

namespace Elefess.Authenticators.GitHub;

/// <summary>
/// Various extension methods for registering <see cref="GitHubLfsAuthenticator"/> instances with an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers a <see cref="GitHubLfsAuthenticator"/> instances with a service collection.
    /// </summary>
    /// <param name="services">The service collection to register the <see cref="GitHubLfsAuthenticator"/> with.</param>
    /// <returns>The service collection with the GitHub authenticator registered.</returns>
    /// <remarks>This overload expects a <see cref="GitHubLfsAuthenticatorOptions"/> instance to also be registered.</remarks>
    public static IServiceCollection AddGitHubAuthenticator(this IServiceCollection services)
    {
        services.AddSingleton<GitHubLfsAuthenticator>();
        services.AddSingleton<ILfsAuthenticator>(static x => x.GetRequiredService<GitHubLfsAuthenticator>());

        return services;
    }
    
    /// <summary>
    /// Registers a <see cref="GitHubLfsAuthenticator"/> instances with a service collection.
    /// </summary>
    /// <param name="services">The service collection to register the <see cref="GitHubLfsAuthenticator"/> with.</param>
    /// <param name="organization">The organization (or user) that the repository belongs to.</param>
    /// <param name="repository">The repository name to authenticate against.</param>
    /// <param name="baseAddress">The base API route when making authentication requests. Defaults to https://api.github.com/.</param>
    /// <returns>The service collection with the GitHub authenticator registered.</returns>
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