using System;
using System.Collections.Generic;
using System.Reflection;

namespace com.liteninja.di.Core.DI
{
    public class Injector : IInjector
    {
        private readonly IReflector _reflector;
        private readonly MethodInfo _getMethodInfo;
        private readonly Dictionary<Type, MethodInfo> _getGenericMethods;
        private readonly IDIContainer _diContainer;

        public Injector(IDIContainer diContainer = null, IReflector reflector = null)
        {
            _diContainer = diContainer ?? new DIContainer();
            _reflector = reflector ?? new CachedReflector();
            _getGenericMethods = new Dictionary<Type, MethodInfo>();
            _getMethodInfo = typeof(IDIContainer).GetMethod("Get");
        }

        public void Inject(object obj)
        {
            var fields = _reflector.GetInjectableFields(obj);
            foreach (var field in fields)
            {
                field.SetValue(obj, GetBindingOf(field.FieldType));
            }

            var properties = _reflector.GetInjectableProperties(obj);
            foreach (var property in properties)
            {
                property.SetValue(obj, GetBindingOf(property.PropertyType));
            }
        }
        
        private object GetBindingOf(Type type)
        {
            if (!_getGenericMethods.ContainsKey(type))
            {
                _getGenericMethods.Add(type, _getMethodInfo.MakeGenericMethod(type));
            }
            return _getGenericMethods[type].Invoke(_diContainer, null);
        }
    }
}