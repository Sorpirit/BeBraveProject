using Core.Data;
using Core.Data.Rooms;
using Core.PlayerSystems;
using Core.RoomsSystem;
using JetBrains.Annotations;
using UnityEngine.Assertions;

namespace Core.GameStates
{
    public class GameContext : IStatesContext
    {
        private bool _isGameStarted;
        private Deck _deck;
        private Dungeon _map;
        private PlayerHand _hand;
        private PlayerPawn _player;
        private IRoomFactory _roomFactory;

        public bool IsGameStarted => _isGameStarted;
        public Deck Deck => _deck;
        public Dungeon Map => _map;
        public PlayerHand Hand => _hand;
        public PlayerPawn Player => _player;
        public IRoomFactory RoomFactory => _roomFactory;
        
        public IState GameStartState { get; private set; }
        public IState FinishGame { get; private set; }
        public IState CheckCardValidity { get; set; }

        public ICardPlacer PlayCardState { get; private set; }
        public IDropCard DropCardState { get; private set; }
        public ITriggerTransition PlaceRoomState { get; private set; }
        public ITriggerTransition PlayerMoveState { get; private set; }
        public ITriggerTransition PlayerEnterRoomState { get; private set; }
        public ITriggerTransition TakeCardState { get; private set; }
        
        public RoomCard? UsedRoomCard { get; set; }
        public Room? CurrentRoom { get; set; }
        [CanBeNull] public IRoomContent CurrentRoomContent { get; set; }
        

        private IState _currentState;

        public void SetStates(IState gameStartState, IState finishGame, IState checkCardValidity, ICardPlacer playCardState, IDropCard dropCardState, ITriggerTransition placeRoomState, ITriggerTransition playerMoveState, ITriggerTransition playerEnterRoomState, ITriggerTransition takeCardState)
        {
            GameStartState = gameStartState;
            FinishGame = finishGame;
            CheckCardValidity = checkCardValidity;
            PlayCardState = playCardState;
            DropCardState = dropCardState;
            PlaceRoomState = placeRoomState;
            PlayerMoveState = playerMoveState;
            PlayerEnterRoomState = playerEnterRoomState;
            TakeCardState = takeCardState;
        }

        public void ChangeState(IState state)
        {
            Assert.IsTrue(_currentState != state);
            _currentState?.ExitState();
            _currentState = state;
            _currentState.EnterState();
        }

        public void InitContext(Deck deck, Dungeon map, PlayerHand hand, PlayerPawn player, IRoomFactory roomFactory)
        {
            _deck = deck;
            _map = map;
            _hand = hand;
            _player = player;
            _roomFactory = roomFactory;
        }
        
        public void StartGame()
        {
            _isGameStarted = true;
        }
    }
}