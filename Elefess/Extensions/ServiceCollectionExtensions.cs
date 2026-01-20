using Microsoft.Extensions.DependencyInjection;

namespace Elefess;

/// <summary>
///     Various extension methods for registering various Elefess types with an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <param name="services">The service collection to register the <see cref="DictionaryLfsAuthenticator"/> with.</param>
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers a <see cref="DictionaryLfsAuthenticator"/> instance with a service collection.
        /// </summary>
        /// <param name="credentials">The credentials passed into the <see cref="DictionaryLfsAuthenticator"/>'s constructor.</param>
        /// <returns>The service collection with the authenticator registered.</returns>
        public IServiceCollection AddDictionaryAuthenticator(IDictionary<string, string> credentials)
            => services.AddWithImplementation<ILfsAuthenticator, DictionaryLfsAuthenticator>(new DictionaryLfsAuthenticator(credentials));

        /// <summary>
        /// Registers a <see cref="BasicLfsTransferRequestSelector"/> instance with a service collection.
        /// </summary>
        /// <returns></returns>
        /// <returns>The service collection with the transfer selector registered.</returns>
        public IServiceCollection AddBasicTransferSelector() => services.AddTransferSelector<BasicLfsTransferRequestSelector>();

        /// <summary>
        /// Registers a <see cref="DefaultLfsObjectManager"/> instance with a service collection.
        /// </summary>
        /// <returns>The service collection with the object manager registered.</returns>
        public IServiceCollection AddDefaultObjectManager() => services.AddObjectManager<DefaultLfsObjectManager>();

        /// <summary>
        /// Registers <see cref="BasicLfsTransferRequestSelector"/> and <see cref="DefaultLfsObjectManager"/> instances with a service collection.
        /// </summary>
        /// <returns>The service collection with the transfer selector and object manager registered.</returns>
        public IServiceCollection AddElefessDefaults() => services.AddBasicTransferSelector().AddDefaultObjectManager();

        /// <summary>
        /// Registers a custom <see cref="ILfsAuthenticator"/> instance with a service collection.
        /// </summary>
        /// <returns>The service collection with the custom authenticator registered.</returns>
        public IServiceCollection AddLfsAuthenticator<TAuthenticator>() where TAuthenticator : class, ILfsAuthenticator
            => services.AddWithImplementation<ILfsAuthenticator, TAuthenticator>();

        /// <summary>
        /// Registers a custom <see cref="ILfsOidMapper"/> instance with a service collection.
        /// </summary>
        /// <returns>The service collection with the custom mapper registered.</returns>
        public IServiceCollection AddOidMapper<TMapper>() where TMapper : class, ILfsOidMapper
            => services.AddWithImplementation<ILfsOidMapper, TMapper>();

        /// <summary>
        /// Registers a custom <see cref="ILfsObjectManager"/> instance with a service collection.
        /// </summary>
        /// <returns>The service collection with the custom object manager registered.</returns>
        public IServiceCollection AddObjectManager<TManager>() where TManager : class, ILfsObjectManager
            => services.AddWithImplementation<ILfsObjectManager, TManager>();

        /// <summary>
        /// Registers a custom <see cref="ILfsRequestValidator"/> instance with a service collection.
        /// </summary>
        /// <returns>The service collection with the custom request validator registered.</returns>
        public IServiceCollection AddRequestValidator<TValidator>() where TValidator : class, ILfsRequestValidator
            => services.AddWithImplementation<ILfsRequestValidator, TValidator>();

        /// <summary>
        /// Registers a custom <see cref="ILfsTransferSelector"/> instance with a service collection.
        /// </summary>
        /// <returns>The service collection with the custom transfer selector registered.</returns>
        public IServiceCollection AddTransferSelector<TSelector>() where TSelector : class, ILfsTransferSelector
            => services.AddWithImplementation<ILfsTransferSelector, TSelector>();

        private IServiceCollection AddWithImplementation<TInterface, TImplementation>(TImplementation? implementation = null)
            where TImplementation : class, TInterface
            where TInterface : class
        {
            if (!typeof(TInterface).IsInterface)
                throw new ArgumentException($"{typeof(TInterface)} must be an interface type.", nameof(TInterface));

            if (implementation is not null)
                services.AddSingleton(implementation);
            else
                services.AddSingleton<TImplementation>();

            services.AddSingleton<TInterface>(static x => x.GetRequiredService<TImplementation>());
            return services;
        }
    }
}