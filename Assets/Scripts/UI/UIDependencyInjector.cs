using Core.RoomsSystem.RoomVariants;
using Game;
using Game.RoomFactories;
using Scripts.DependancyInjector;
using UnityEngine;

namespace UI
{
    public class UIDependencyInjector : MonoDependencyInjector
    {
        [SerializeField] private BasicIconFactory basicIconFactory;
        [SerializeField] private GridPositionConvertor positionConvertor;
        [SerializeField] private FightingEncounterFactory fightingEncounterFactory;
        
        protected override void InstallBindings(IObjectDependencyInjector injector)
        {
            injector.RegisterSingle<IItemIconFactory, BasicIconFactory>(basicIconFactory);
            injector.RegisterSingle<IRoomPositionConvertor, GridPositionConvertor>(positionConvertor);
            injector.RegisterSingle<IFightCallbacks, FightingEncounterFactory>(fightingEncounterFactory);
        }
    }
}