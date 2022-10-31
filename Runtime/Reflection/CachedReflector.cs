using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteNinja.DI
{
    
    public class CachedReflector : IReflector
    {
        private readonly BaseReflector _reflector;
        private readonly Dictionary<System.Type, FieldInfo[]> _cachedFields;
        private readonly Dictionary<System.Type, PropertyInfo[]> _cachedProperties;

        public CachedReflector()
        {
            _reflector = new BaseReflector();
            _cachedFields = new Dictionary<Type, FieldInfo[]>();
            _cachedProperties = new Dictionary<Type, PropertyInfo[]>();
        }
        
        public IEnumerable<FieldInfo> GetInjectableFields(object obj)
        {
            var objectType = obj.GetType();
            if (!_cachedFields.ContainsKey(objectType))
            {
                _cachedFields.Add(objectType, _reflector.GetInjectableFields(obj).ToArray());
            }
            return _cachedFields[objectType];
        }

        public IEnumerable<PropertyInfo> GetInjectableProperties(object obj)
        {
            var objectType = obj.GetType();
            if (!_cachedProperties.ContainsKey(objectType))
            {
                _cachedProperties.Add(objectType, _reflector.GetInjectableProperties(obj).ToArray());
            }
            return _cachedProperties[objectType];
        }
    }
}