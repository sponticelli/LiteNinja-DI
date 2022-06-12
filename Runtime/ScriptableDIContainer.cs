using System;
using UnityEngine;

namespace com.liteninja.di.Core.DI
{
    [CreateAssetMenu(menuName = "LiteNinja/DI/ScriptableDIContainer", fileName = "ScriptableDIContainer", order = 0)]
    public class ScriptableDIContainer : ScriptableObject, IDIContainer
    {
        [SerializeField] private ScriptableDIContainer _parentContainer;
        [NonSerialized] private bool initialized;
        [NonSerialized] private IDIContainer _container;

        public IDIContainer Container
        {
            get
            {
                if (initialized) return _container;
                _container = _parentContainer != null ? new DIContainer(_parentContainer.Container) : new DIContainer();
                initialized = true;
                return _container;
            }
        }

        public void Bind<T>(IServiceProvider<T> provider, bool overrideExisting = true) where T : class
        {
            Container.Bind(provider, overrideExisting);
        }

        public void Bind(IServiceProvider provider, bool overrideExisting = true)
        {
            Container.Bind(provider, overrideExisting);
        }

        public void BindInstance<T>(T service, Action onTearDown = null, bool overrideExisting = true) where T : class
        {
            Container.BindInstance(service, onTearDown, overrideExisting);
        }

        public void BindFactory<T>(Func<T> factory, Action onTearDown = null, bool overrideExisting = true)
            where T : class
        {
            Container.BindFactory(factory, onTearDown, overrideExisting);
        }

        public bool Unbind<T>(IServiceProvider<T> provider = null, bool tearDown = true) where T : class
        {
            return Container.Unbind<T>(provider, tearDown);
        }

        public bool Unbind(Type type, bool tearDown = true)
        {
            return Container.Unbind(type, tearDown);
        }

        public bool Unbind(IServiceProvider provider, bool tearDown = true)
        {
            return Container.Unbind(provider, tearDown);
        }

        public void UnbindAll(bool tearDown = true)
        {
            Container.UnbindAll();
        }

        public bool IsBound<T>() where T : class
        {
            return Container.IsBound<T>();
        }

        public bool IsBound(Type type)
        {
            return Container.IsBound(type);
        }

        public bool Exists<T>() where T : class
        {
            return Container.Exists<T>();
        }

        public bool Exists(Type type)
        {
            return Container.Exists(type);
        }

        public bool CanGet<T>() where T : class
        {
            return Container.CanGet<T>();
        }

        public bool CanGet(Type type)
        {
            return Container.CanGet(type);
        }

        public T Get<T>() where T : class
        {
            return Container.Get<T>();
        }

        public T GetIfBound<T>() where T : class
        {
            return Container.GetIfBound<T>();
        }

        public T GetIfExists<T>() where T : class
        {
            return Container.GetIfExists<T>();
        }
    }
}