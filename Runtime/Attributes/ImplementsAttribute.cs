using System;
using UnityEngine;

namespace LiteNinja.DI.attributes
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