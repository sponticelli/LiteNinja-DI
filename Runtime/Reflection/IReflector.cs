using System.Collections.Generic;
using System.Reflection;

namespace com.liteninja.di.Core.DI
{
    public interface IReflector
    {
        IEnumerable<FieldInfo> GetInjectableFields(object obj);
        IEnumerable<PropertyInfo> GetInjectableProperties(object obj);
    }
}