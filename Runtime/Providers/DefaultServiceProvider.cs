using System;

namespace com.liteninja.di.Core.DI
{
    public class DefaultServiceProvider<T> : IServiceProvider<T> where T : class
    {
        private T _instance;
        private readonly Func<T> _factory;
        private readonly Action _onTearDown;

        public DefaultServiceProvider(Func<T> factory, Action onTearDown = null) {
            _factory = factory;
            _onTearDown = onTearDown;
            _instance = null;
        }

        public Type ServiceType => typeof(T);
        
        public T Get() {
            return _instance ??= _factory();
        }

        public bool CanGet() {
            return _factory != null;
        }

        public bool Exists() {
            return _instance != null;
        }

        public void TearDown() {
            _onTearDown?.Invoke();
            _instance = null;
        }
    }

    
}