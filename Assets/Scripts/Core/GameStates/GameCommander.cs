using Library.GameFlow.StateSystem;
using UnityEngine.Assertions;

namespace Core.GameStates
{
    public class GameCommander : IStateSwitcher
    {
        public IState GameStartState { get; private set; }
        public IState FinishGame { get; private set; }
        public IState CheckCardValidity { get; private set; }

        public ICardPlacer PlayCardState { get; private set; }
        public IDropCard DropCardState { get; private set; }
        public ITriggerTransition PlaceRoomState { get; private set; }
        public ITriggerTransition PlayerMoveState { get; private set; }
        public ITriggerTransition PlayerEnterRoomState { get; private set; }
        public ITriggerTransition TakeCardState { get; private set; }
        
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
            Assert.IsNotNull(state);
            _currentState?.ExitState();
            _currentState = state;
            _currentState.EnterState();
        }
    }
}