using Game;
using Scripts.DependancyInjector;
using UnityEngine;

namespace UI
{
    public class UIDependencyInjector : MonoDependencyInjector
    {
        [SerializeField] private BasicIconFactory basicIconFactory;
        [SerializeField] private GridPositionConvertor positionConvertor;
        
        protected override void InstallBindings(IObjectDependencyInjector injector)
        {
            injector.RegisterSingle<IItemIconFactory, BasicIconFactory>(basicIconFactory);
            injector.RegisterSingle<IRoomPositionConvertor, GridPositionConvertor>(positionConvertor);
        }
    }
}