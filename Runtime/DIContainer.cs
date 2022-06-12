using System;
using System.Collections.Generic;
using System.Reflection;

namespace com.liteninja.di.Core.DI
{
    public class DIContainer : IDIContainer
    {
        private readonly Dictionary<Type, IServiceProvider> _serviceProviders;
        private readonly IDIContainer _parent;
        

        public DIContainer(IDIContainer parent = null)
        {
            _parent = parent;
            _serviceProviders = new Dictionary<Type, IServiceProvider>();
            BindInstance<IDIContainer>(this);
        }
        

        public void Bind<T>(IServiceProvider<T> provider, bool overrideExisting = true)
            where T : class
        {
            Bind((IServiceProvider)provider, overrideExisting);
        }

        public void Bind(IServiceProvider provider, bool overrideExisting = true)
        {
            if (IsBound(provider.ServiceType))
            {
                if (overrideExisting)
                {
                    Unbind(provider.ServiceType);
                }
                else
                {
                    throw new ArgumentException(
                        $"Service of type {provider.ServiceType.Name} is already registered");
                }
            }

            _serviceProviders.Add(provider.ServiceType, provider);
        }

        public void BindInstance<T>(T service, Action onTearDown = null, bool overrideExisting = true)
            where T : class
        {
            BindFactory(() => service, onTearDown, overrideExisting);
        }

        public void BindFactory<T>(Func<T> factory, Action onTearDown = null,
            bool overrideExisting = true) where T : class
        {
            Bind(new DefaultServiceProvider<T>(factory, onTearDown), overrideExisting);
        }

        public bool Unbind<T>(IServiceProvider<T> provider = null, bool tearDown = true) where T : class
        {
            return provider != null
                ? Unbind((IServiceProvider)provider, tearDown)
                : Unbind(typeof(T), tearDown);
        }

        public bool Unbind(Type type, bool tearDown = true)
        {
            return _serviceProviders.TryGetValue(type, out var provider) && RemoveInternal(provider, tearDown);
        }

        public bool Unbind(IServiceProvider serviceProvider, bool tearDown = true)
        {
            if (!_serviceProviders.TryGetValue(serviceProvider.ServiceType, out var provider))
            {
                return false;
            }

            return provider == serviceProvider && RemoveInternal(serviceProvider, tearDown);
        }

        public void UnbindAll(bool tearDown = true)
        {
            if (tearDown)
            {
                foreach (var provider in _serviceProviders.Values)
                {
                    provider.TearDown();
                }
            }

            _serviceProviders.Clear();
        }

        public bool IsBound<T>() where T : class
        {
            return IsBound(typeof(T));
        }

        public bool IsBound(Type type)
        {
            var local = _serviceProviders.ContainsKey(type);
            if (HasParent())
            {
                return local || _parent.IsBound(type);
            }

            return _serviceProviders.ContainsKey(type);
        }

        public bool Exists<T>() where T : class
        {
            return Exists(typeof(T));
        }

        public bool Exists(Type type)
        {
            var local = _serviceProviders.TryGetValue(type, out var provider) && provider.Exists();
            if (HasParent())
            {
                return local || _parent.Exists(type);
            }

            return local;
        }

        public bool CanGet<T>() where T : class
        {
            return CanGet(typeof(T));
        }

        public bool CanGet(Type type)
        {
            var local = _serviceProviders.TryGetValue(type, out var provider) && provider.CanGet();
            if (HasParent())
            {
                return local || _parent.CanGet(type);
            }

            return local;
        }

        public T Get<T>() where T : class
        {
            ArgumentException exp = null;

            if (_serviceProviders.TryGetValue(typeof(T), out var provider))
            {
                if (provider.CanGet())
                {
                    var typedProvider = (IServiceProvider<T>)provider;
                    var service = typedProvider.Get();
                    if (service == null)
                        throw new ArgumentException($"Service of type {typeof(T).Name} is null");
                    return service;
                }

                exp = new ArgumentException(
                    $"Service provider for type {typeof(T).Name} cannot get a service instance");
            }
            else
            {
                exp = new ArgumentException($"No Service provider of type {typeof(T).Name} found");
            }

            if (HasParent())
            {
                return _parent.Get<T>();
            }

            throw exp;
        }

        public T GetIfBound<T>() where T : class
        {
            return IsBound<T>() ? Get<T>() : null;
        }

        public T GetIfExists<T>() where T : class
        {
            return Exists<T>() ? Get<T>() : null;
        }

        private bool HasParent()
        {
            return _parent != null;
        }

        private bool RemoveInternal(IServiceProvider provider, bool tearDown)
        {
            if (tearDown)
            {
                provider.TearDown();
            }

            return _serviceProviders.Remove(provider.ServiceType);
        }
    }
}