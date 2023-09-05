using System;

namespace Scripts.DependancyInjector
{
    public interface IObjectDependencyInjector
    {
        public delegate object Create();

        public bool RegisterSingle<T, S>(S service) where S : T;

        public bool RegisterSingle<T>(Create factoryMethod);

        public bool Register<T>(Create factoryMethod);

        public T Resolve<T>();

        public object Resolve(Type type);

        public void Inject(object obj);
    }
}