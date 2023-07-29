using System;
using UnityEngine;

namespace LiteNinja.DI
{
    public class MonoScriptableInjector : MonoBehaviour
    {
        [SerializeField] private ScriptableInjector _injector;
        [SerializeField] private bool _injectChildren;
        [SerializeField] private bool _includInactives = true;
        [SerializeField] private bool _injectOnAwake;
        [SerializeField] private bool _injectOnEnable;
        [SerializeField] private bool _injectOnStart;
        
        private Component[] _components;
        
        private bool _isDestroyed;
        
        private void Awake()
        {
            _components = _injectChildren
                ? GetComponentsInChildren(typeof(Component), _includInactives)
                : GetComponents(typeof(Component));

            if (_injectOnAwake) Inject();
        }

        private void OnDestroy()
        {
            _isDestroyed = true;
        }

        private void OnEnable()
        {
            if (_injectOnEnable) Inject();
        }
        
        private void Start()
        {
            if (_injectOnStart) Inject();
        }


        public void Inject()
        {
            _isDestroyed = false;
            foreach (var component in _components)
            {
                _injector.Inject(component);
            }
        }
    }
}