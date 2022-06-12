using System;
using UnityEngine;

namespace com.liteninja.di.Core.DI.Attributes
{
    public class ImplementsAttribute : PropertyAttribute {
        public Type InterfaceType { get; }

        public ImplementsAttribute(Type interfaceType) {
            if (interfaceType == null) {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (!interfaceType.IsInterface) {
                throw new ArgumentException($"{nameof(interfaceType)} must be an interface type");
            }

            InterfaceType = interfaceType;
        }
    }
}