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
        [SerializeField] private RoomFactory factory;
        [SerializeField] private FightingEncounterFactory fightingEncounterFactory;
        [SerializeField] private ItemRoomFactory itemRoomFactory;
        [SerializeField] private CameraEffects cameraEffects;
        
        protected override void InstallBindings(IObjectDependencyInjector injector)
        {
            injector.RegisterSingle<IItemIconFactory, BasicIconFactory>(basicIconFactory);
            injector.RegisterSingle<IRoomPositionConvertor, GridPositionConvertor>(positionConvertor);
            
            injector.RegisterSingle<IFightCallbacks, FightingEncounterFactory>(fightingEncounterFactory);
            injector.RegisterSingle<IRoomContentCallBack<FightEncounterContext>, FightingEncounterFactory>(fightingEncounterFactory);
            
            injector.RegisterSingle<IPickUpCallbacks, ItemRoomFactory>(itemRoomFactory);
            injector.RegisterSingle<IRoomContentCallBack<ItemRoomContext>, ItemRoomFactory>(itemRoomFactory);
            
            injector.RegisterSingle<IRoomContentCallBack<EmptyRoomContext>, RoomFactory>(factory);
            
            injector.RegisterSingle<ICameraEffects, CameraEffects>(cameraEffects);
        }
    }
}