using Scripts.DependancyInjector;
using UnityEngine;

namespace UI
{
    public class UIDependencyInjector : MonoDependencyInjector
    {
        [SerializeField] private BasicIconFactory basicIconFactory;
        
        protected override void InstallBindings(IObjectDependencyInjector injector)
        {
            injector.RegisterSingle<IItemIconFactory, BasicIconFactory>(basicIconFactory);
        }
    }
}