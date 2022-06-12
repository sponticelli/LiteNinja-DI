using System;
using UnityEngine;

namespace com.liteninja.di.Core.DI
{
    public class MonoScriptableInjector : MonoBehaviour
    {
        [SerializeField] private ScriptableInjector _injector;
        [SerializeField] private bool _injectChildren;
        
        private void Awake()
        {
            var components = _injectChildren
                ? GetComponentsInChildren(typeof(Component))
                : GetComponents(typeof(Component));

            foreach (var component in components)
            {
                _injector.Inject(component);
            }
        }
    }
}