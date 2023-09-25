using Core.Data.Scriptable;
using Core.GameStates.States;
using Core.RoomsSystem;

namespace Core.GameStates
{
    public static class GameStateSetup
    {
        public static GameCommander SetupStates(GameSetup setup, GameContext context)
        {
            GameCommander commander = new GameCommander();

            GameStartState gameStartState = new GameStartState(setup, context, commander);
            FinishState gameFinishState = new FinishState(context, commander);
            CheckCardsValid checkCardsValid = new CheckCardsValid(context, commander);

            TakeCardState takeCardState = new TakeCardState(context, commander);
            DropCardState dropCardState = new DropCardState(context, commander);
            
            PlayCardState playCardState = new PlayCardState(context, commander);
            RoomContentPlacerState placeRoomState = new RoomContentPlacerState(context, commander);
            PlayerMoveState playerMoveState = new PlayerMoveState(context, commander);
            EnterRoomState playerEnterRoomState = new EnterRoomState(context, commander);
            
            gameStartState.NextState = checkCardsValid;
            checkCardsValid.PlayCardState = playCardState;
            checkCardsValid.DropCardState = dropCardState;
            
            takeCardState.NextState = checkCardsValid;
            takeCardState.FinishGame = gameFinishState;
            dropCardState.NextState = takeCardState;
            
            playCardState.NextState = placeRoomState;
            placeRoomState.NextState = playerMoveState;
            playerMoveState.NextState = playerEnterRoomState;
            playerEnterRoomState.NextState = takeCardState;
            playerEnterRoomState.FinishGame = gameFinishState;
            
            commander.SetStates(
                gameStartState, 
                gameFinishState,
                checkCardsValid,
                playCardState, 
                dropCardState,
                placeRoomState, 
                playerMoveState,
                playerEnterRoomState, 
                takeCardState);
            
            return commander;
        }
    }
}