using Microsoft.Extensions.DependencyInjection;

namespace Elefess.Extensions;

/// <summary>
/// Various extension methods for registering various Elefess types with an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers a <see cref="DictionaryLfsAuthenticator"/> instance with a service collection.
    /// </summary>
    /// <param name="services">The service collection to register the <see cref="DictionaryLfsAuthenticator"/> with.</param>
    /// <param name="credentials">The credentials passed into the <see cref="DictionaryLfsAuthenticator"/>'s constructor.</param>
    /// <returns>The service collection with the authenticator registered.</returns>
    public static IServiceCollection AddDictionaryAuthenticator(this IServiceCollection services, IDictionary<string, string> credentials)
    {
        services.AddSingleton(new DictionaryLfsAuthenticator(credentials));
        services.AddSingleton<ILfsAuthenticator>(static x => x.GetRequiredService<DictionaryLfsAuthenticator>());
        return services;
    }
    
    /// <summary>
    /// Registers a <see cref="BasicLfsTransferRequestSelector"/> instance with a service collection.
    /// </summary>
    /// <param name="services">The service collection to register the <see cref="BasicLfsTransferRequestSelector"/> with.</param>
    /// <returns></returns>
    /// <returns>The service collection with the transfer selector registered.</returns>
    public static IServiceCollection AddBasicTransferSelector(this IServiceCollection services)
        => services.AddTransferSelector<BasicLfsTransferRequestSelector>();

    /// <summary>
    /// Registers a <see cref="DefaultLfsObjectManager"/> instance with a service collection.
    /// </summary>
    /// <param name="services">The service collection to register the <see cref="DefaultLfsObjectManager"/> with.</param>
    /// <returns>The service collection with the object manager registered.</returns>
    public static IServiceCollection AddDefaultObjectManager(this IServiceCollection services)
        => services.AddObjectManager<DefaultLfsObjectManager>();
    
    /// <summary>
    /// Adds <see cref="BasicLfsTransferRequestSelector"/> and <see cref="DefaultLfsObjectManager"/> instances with a service collection.
    /// </summary>
    /// <param name="services">The service collection to register the <see cref="BasicLfsTransferRequestSelector"/> and <see cref="DefaultLfsObjectManager"/> with.</param>
    /// <returns>The service collection with the transfer selector and object manager registered.</returns>
    public static IServiceCollection AddElefessDefaults(this IServiceCollection services)
    {
        services.AddBasicTransferSelector();
        services.AddDefaultObjectManager();
        return services;
    }

    /// <summary>
    /// Registers a custom <see cref="ILfsAuthenticator"/> instance with a service collection.
    /// </summary>
    /// <param name="services">The service collection to register the <see cref="ILfsAuthenticator"/> with.</param>
    /// <returns>The service collection with the custom authenticator registered.</returns>
    public static IServiceCollection AddLfsAuthenticator<TAuthenticator>(this IServiceCollection services)
        where TAuthenticator : class, ILfsAuthenticator
    {
        return services.AddWithImplementation<ILfsAuthenticator, TAuthenticator>();
    }

    /// <summary>
    /// Registers a custom <see cref="ILfsOidMapper"/> instance with a service collection.
    /// </summary>
    /// <param name="services">The service collection to register the <see cref="ILfsOidMapper"/> with.</param>
    /// <returns>The service collection with the custom mapper registered.</returns>
    public static IServiceCollection AddOidMapper<TMapper>(this IServiceCollection services)
        where TMapper : class, ILfsOidMapper
    {
        return services.AddWithImplementation<ILfsOidMapper, TMapper>();
    }

    /// <summary>
    /// Registers a custom <see cref="ILfsObjectManager"/> instance with a service collection.
    /// </summary>
    /// <param name="services">The service collection to register the <see cref="ILfsObjectManager"/> with.</param>
    /// <returns>The service collection with the custom object manager registered.</returns>
    public static IServiceCollection AddObjectManager<TManager>(this IServiceCollection services)
        where TManager : class, ILfsObjectManager
    {
        return services.AddWithImplementation<ILfsObjectManager, TManager>();
    }

    /// <summary>
    /// Registers a custom <see cref="ILfsRequestValidator"/> instance with a service collection.
    /// </summary>
    /// <param name="services">The service collection to register the <see cref="ILfsRequestValidator"/> with.</param>
    /// <returns>The service collection with the custom request validator registered.</returns>
    public static IServiceCollection AddRequestValidator<TValidator>(this IServiceCollection services)
        where TValidator : class, ILfsRequestValidator
    {
        return services.AddWithImplementation<ILfsRequestValidator, TValidator>();
    }

    /// <summary>
    /// Registers a custom <see cref="ILfsTransferSelector"/> instance with a service collection.
    /// </summary>
    /// <param name="services">The service collection to register the <see cref="ILfsTransferSelector"/> with.</param>
    /// <returns>The service collection with the custom transfer selector registered.</returns>
    public static IServiceCollection AddTransferSelector<TSelector>(this IServiceCollection services)
        where TSelector : class, ILfsTransferSelector
    {
        return services.AddWithImplementation<ILfsTransferSelector, TSelector>();
    }

    private static IServiceCollection AddWithImplementation<TInterface, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TInterface
        where TInterface : class
    {
        if (!typeof(TInterface).IsInterface)
            throw new ArgumentException($"{typeof(TInterface)} must be an interface type.", nameof(TInterface));

        services.AddSingleton<TImplementation>();
        services.AddSingleton<TInterface>(static x => x.GetRequiredService<TImplementation>());
        return services;
    }
}