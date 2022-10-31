using System;
using UnityEngine;

namespace LiteNinja.DI
{
    [CreateAssetMenu(menuName = "LiteNinja/DI/ScriptableInjector", fileName = "ScriptableInjector", order = 0)]
    public class ScriptableInjector : ScriptableObject, IInjector
    {
        [SerializeField] private ScriptableDIContainer _container;
                
        [NonSerialized] private IInjector _injector;
        [NonSerialized] private bool initialized;

        public void Inject(object obj)
        {
            if (!initialized)
            {
                _injector = new Injector(_container?.Container, bindToSelf: true);
                initialized = true;
            }
            _injector.Inject(obj);
        }
    }
}