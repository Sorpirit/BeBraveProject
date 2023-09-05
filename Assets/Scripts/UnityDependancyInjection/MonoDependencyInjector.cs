using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Scripts.DependancyInjector
{
    public abstract class MonoDependencyInjector : MonoBehaviour
    {
        private IObjectDependencyInjector _objectDependencyInjector;

        public void Awake()
        {
            _objectDependencyInjector = new ObjectDependencyInjector();
            InstallBindings(_objectDependencyInjector);

            var children = GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var child in children)
            {
                _objectDependencyInjector.Inject(child);
            }
        }
        
        protected abstract void InstallBindings(IObjectDependencyInjector injector);
    }
}
