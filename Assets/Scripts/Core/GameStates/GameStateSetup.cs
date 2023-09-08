using Core.Data.Scriptable;
using Core.GameStates.States;
using Core.RoomsSystem;

namespace Core.GameStates
{
    public static class GameStateSetup
    {
        public static GameContext CreateBasicGame(CardSet cardSet, IRoomFactory roomContent)
        {
            GameContext resultContext = new GameContext();

            GameStartState gameStartState = new GameStartState(resultContext, cardSet, roomContent);
            FinishState gameFinishState = new FinishState(resultContext);
            CheckCardsValid checkCardsValid = new CheckCardsValid(resultContext);

            TakeCardState takeCardState = new TakeCardState(resultContext);
            DropCardState dropCardState = new DropCardState(resultContext);
            PlayCardState playCardState = new PlayCardState(resultContext);
            RoomContentPlacerState placeRoomState = new RoomContentPlacerState(resultContext);
            PlayerMoveState playerMoveState = new PlayerMoveState(resultContext);
            EnterRoomState playerEnterRoomState = new EnterRoomState(resultContext);

            resultContext.SetStates(
                gameStartState, 
                gameFinishState,
                checkCardsValid,
                playCardState, 
                dropCardState,
                placeRoomState, 
                playerMoveState,
                playerEnterRoomState, 
                takeCardState);
            
            return resultContext;
        }
    }
}