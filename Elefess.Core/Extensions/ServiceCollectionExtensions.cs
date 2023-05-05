using Microsoft.Extensions.DependencyInjection;

namespace Elefess.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBasicTransferAdapter(this IServiceCollection services)
        => services.AddTransferAdapter<BasicLfsTransferRequestAdapter>();

    public static IServiceCollection AddDefaultObjectManager(this IServiceCollection services)
        => services.AddObjectManager<DefaultLfsObjectManager>();
    
    public static IServiceCollection AddElefessDefaults(this IServiceCollection services)
    {
        services.AddBasicTransferAdapter();
        services.AddDefaultObjectManager();
        return services;
    }

    public static IServiceCollection AddLfsAuthenticator<TAuthenticator>(this IServiceCollection services)
        where TAuthenticator : class, ILfsAuthenticator
    {
        return services.AddWithImplementation<ILfsAuthenticator, TAuthenticator>();
    }

    public static IServiceCollection AddOidMapper<TMapper>(this IServiceCollection services)
        where TMapper : class, ILfsOidMapper
    {
        return services.AddWithImplementation<ILfsOidMapper, TMapper>();
    }

    public static IServiceCollection AddObjectManager<TManager>(this IServiceCollection services)
        where TManager : class, ILfsObjectManager
    {
        return services.AddWithImplementation<ILfsObjectManager, TManager>();
    }

    public static IServiceCollection AddRequestValidator<TValidator>(this IServiceCollection services)
        where TValidator : class, ILfsRequestValidator
    {
        return services.AddWithImplementation<ILfsRequestValidator, TValidator>();
    }

    public static IServiceCollection AddTransferAdapter<TAdapter>(this IServiceCollection services)
        where TAdapter : class, ILfsTransferAdapter
    {
        return services.AddWithImplementation<ILfsTransferAdapter, TAdapter>();
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