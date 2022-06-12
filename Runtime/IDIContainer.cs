using System;

namespace com.liteninja.di
{
    public interface IDIContainer
    {
        /// <summary>
        /// Bind a service provider for type T
        /// </summary>
        /// <param name="provider">The service provider to register</param>
        /// <param name="overrideExisting">If true, override existing service provider of the same T. Otherwise throw exception if service provider is already registered</param>
        /// <typeparam name="T">The type of the service</typeparam>
        /// <exception cref="ArgumentException">If a service provider for the same T is already registered and overrideExisting == false</exception>
        void Bind<T>(IServiceProvider<T> provider, bool overrideExisting = true)
            where T : class;

        void Bind(IServiceProvider provider, bool overrideExisting = true);

        /// <summary>
        /// Bind an instance of a service using a default service provider
        /// </summary>
        /// <param name="service">The service instance</param>
        /// <param name="onTearDown">Called when the service is removed</param>
        /// <param name="overrideExisting">If true, override existing service provider of the same T. Otherwise throw exception if service provider is already registered</param>
        /// <typeparam name="T">The type of the service to expose</typeparam>
        /// <exception cref="ArgumentException">If a service provider of type T is already registered and overrideExisting == false</exception>
        void BindInstance<T>(T service, Action onTearDown = null, bool overrideExisting = true) where T : class;

        /// <summary>
        /// Bind a service factory using a default service provider. The service is instantiated lazily.
        /// </summary>
        /// <param name="factory">Function that creates a service instance</param>
        /// <param name="onTearDown">Called when the service is removed</param>
        /// <param name="overrideExisting">If true, override existing service provider of the same T. Otherwise throw exception if service provider is already registered</param>
        /// <typeparam name="T">The type of the service to expose</typeparam>
        /// <exception cref="ArgumentException">If a service provider of type T is already registered and overrideExisting == false</exception>
        void BindFactory<T>(Func<T> factory, Action onTearDown = null, bool overrideExisting = true) where T : class;

        /// <summary>
        /// Unbind a service provider of type T. If provider != null, removes it only if it is in fact the same object.
        /// Otherwise, remove whatever service provider is providing T.
        /// </summary>
        /// <param name="provider">Unbind this specific service provider. If null, remove the service provider for type T</param>
        /// <param name="tearDown">Whether to call TearDown() on the removed service provider (default: true)</param>
        /// <typeparam name="T">The type of the service to remove</typeparam>
        /// <returns>true if the service provider was actually removed</returns>
        bool Unbind<T>(IServiceProvider<T> provider = null, bool tearDown = true) where T : class;

        bool Unbind(Type type, bool tearDown = true);

        bool Unbind(IServiceProvider provider, bool tearDown = true);

        /// <summary>
        /// Unbind all service providers.
        /// </summary>
        /// <param name="tearDown">Whether to call TearDown() on each removed service provider (default: true)</param>
        void UnbindAll(bool tearDown = true);

        /// <summary>
        /// Check if type T has a service provider registered for it.
        /// </summary>
        /// <typeparam name="T">The service type to check</typeparam>
        /// <returns>true if there is a service provider for type T</returns>
        bool IsBound<T>() where T : class;

        /// <summary>
        /// Check if type has a service provider registered for it.
        /// </summary>
        /// <param name="type">The service type to check</param>
        /// <returns>true if there is a service provider for type</returns>
        bool IsBound(Type type);

        /// <summary>
        /// Check whether a service provider for type T is registered and has already created an instance of the service.
        /// This can be used in cleanup code (e.g. OnDestroy) to avoid creating a service instance if it wasn't already.
        /// </summary>
        /// <typeparam name="T">The service type to check</typeparam>
        /// <returns>true if an instance of the service T has already been created</returns>
        bool Exists<T>() where T : class;

        /// <summary>
        /// Check whether a service provider for type is registered and has already created an instance of the service.
        /// This can be used in cleanup code (e.g. OnDestroy) to avoid creating a service instance if it wasn't already.
        /// </summary>
        /// <param name="type">The service type to check</param>
        /// <returns>true if an instance of the service has already been created</returns>
        bool Exists(Type type);

        /// <summary>
        /// Check whether a call to Get&lt;T&gt;() would be successful.
        /// </summary>
        /// <typeparam name="T">The service type to check</typeparam>
        /// <returns>true if a call to Get&lt;T&gt;() would succeed</returns>
        bool CanGet<T>() where T : class;

        /// <summary>
        /// Check whether a call to Get&lt;T&gt;() would be successful.
        /// </summary>
        /// <param name="type">The service type to check</param>
        /// <returns>true if a call to Get&lt;T&gt;() would succeed</returns>
        bool CanGet(Type type);

        /// <summary>
        /// Get an instance of a service of type T, creating it if necessary.
        /// </summary>
        /// <typeparam name="T">The service type to get</typeparam>
        /// <returns>The service instance</returns>
        /// <exception cref="ArgumentException">If no service provider of type T is registered, or ServiceProvider.CanGet() == false,
        /// or the service instance returned by the service provider is null</exception>
        T Get<T>() where T : class;

        /// <summary>
        /// Get an instance of a service of type T, only if a corresponding service provider is registered.
        /// Creates the instance if necessary.
        /// </summary>
        /// <typeparam name="T">The service type to get</typeparam>
        /// <returns>The service instance, or null if it is not registered</returns>
        T GetIfBound<T>() where T : class;

        /// <summary>
        /// Get an instance of a service of type T, only if an instance already exists.
        /// It never creates an instance of the service, unless one exists already.
        /// </summary>
        /// <typeparam name="T">The service type to get</typeparam>
        /// <returns>The service instance, or null if it doesn't exist</returns>
        T GetIfExists<T>() where T : class;
    }
}