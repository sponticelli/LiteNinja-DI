using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LiteNinja.DI.Attributes;

namespace LiteNinja.DI
{
    /// <summary>
    /// Utility class to extract fields and properties marked with [Inject] Attribute
    /// </summary>
    public class BaseReflector : IReflector
    {
        private readonly List<FieldInfo> _fieldInfoList;
        private readonly List<PropertyInfo> _propertyInfoList;
        public BaseReflector()
        {
            _fieldInfoList = new List<FieldInfo>(256);
            _propertyInfoList = new List<PropertyInfo>(256);
        }
        
        
        public IEnumerable<FieldInfo> GetInjectableFields(object obj)
        {
            _fieldInfoList.Clear();
            var objectType = obj.GetType();
            var fields = objectType.GetRuntimeFields();
            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes<InjectAttribute>();
                if (!attributes.Any()) continue;
                _fieldInfoList.Add(field);
            }

            return _fieldInfoList.ToArray();
        }

        public IEnumerable<PropertyInfo> GetInjectableProperties(object obj)
        {
            _propertyInfoList.Clear();
            var objectType = obj.GetType();
            var properties = objectType.GetRuntimeProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes<InjectAttribute>();
                if (!attributes.Any()) continue;
                _propertyInfoList.Add(property);
            }

            return _propertyInfoList.ToArray();
        }
    }
}