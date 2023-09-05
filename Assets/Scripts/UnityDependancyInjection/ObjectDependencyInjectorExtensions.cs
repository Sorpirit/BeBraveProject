using UnityEngine;

namespace Scripts.DependancyInjector
{
    public static class ObjectDependencyInjectorExtensions
    {
        public static bool RegisterPrefab<T>(this IObjectDependencyInjector injector, GameObject prefab) where T : MonoBehaviour
        {
            return injector.Register<T>(() =>
            {
                var gameObject = GameObject.Instantiate(prefab);
                return gameObject.GetComponent<T>();
            });
        }
    }
}