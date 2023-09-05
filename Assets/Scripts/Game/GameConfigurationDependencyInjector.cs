using Core.RoomsSystem;
using Game.RoomFactories;
using Scripts.DependancyInjector;
using UnityEngine;

namespace Game
{
    public class GameConfigurationDependencyInjector : MonoDependencyInjector
    {
        [SerializeField] 
        private RoomFactory factory;
        
        protected override void InstallBindings(IObjectDependencyInjector injector)
        {
            injector.RegisterSingle<IRoomFactory, RoomFactory>(factory);
            injector.RegisterSingle<IFactoryAggregator, RoomFactory>(factory);
        }
    }
}