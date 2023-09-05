using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Scripts.DependancyInjector
{
    public class ObjectDependencyInjector : IObjectDependencyInjector
    {
        private readonly ConcurrentDictionary<Type, object> _singleton = new();
        private readonly ConcurrentDictionary<Type, IObjectDependencyInjector.Create> _singletonFactory = new();
        private readonly ConcurrentDictionary<Type, IObjectDependencyInjector.Create> _objectFactory = new();
        
        
        public bool RegisterSingle<T, S>(S service) where S : T
        {
            return _singleton.TryAdd(typeof(T), service);
        }
        
        public bool RegisterSingle<T>(IObjectDependencyInjector.Create factoryMethod)
        {
            return _singletonFactory.TryAdd(typeof(T), factoryMethod);
        }
        
        public bool Register<T>(IObjectDependencyInjector.Create factoryMethod)
        {
            return _objectFactory.TryAdd(typeof(T), factoryMethod);
        }

        public T Resolve<T>()
        {
            return (T) Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            if (_singleton.TryGetValue(type, out var result))
                return result;

            if (_singletonFactory.TryGetValue(type, out var factoryMethod))
            {
                return _singleton.GetOrAdd(type, _ =>
                {
                    var resolved = factoryMethod();
                    Inject(resolved);
                    return resolved;
                });
            }

            if (_objectFactory.TryGetValue(type, out factoryMethod))
            {
                var resolved = factoryMethod();
                Inject(resolved);
                return resolved;
            }

            throw new ArgumentException($"Unknown field type. Nothing is registered in DI container for {type}");
        }
        
        public void Inject(object obj)
        {
            var type = obj.GetType();

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic
                                                            | BindingFlags.DeclaredOnly | BindingFlags.Instance).Where(field => field.IsDefined(typeof(InjectAttribute), false));
            foreach (var field in fields)
            {
                var resolved = Resolve(field.FieldType);
                field.SetValue(obj, resolved);
            }
        }
    }
}