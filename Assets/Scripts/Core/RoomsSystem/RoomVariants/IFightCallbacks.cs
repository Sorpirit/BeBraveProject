using System;

namespace Core.RoomsSystem.RoomVariants
{
    public interface IFightCallbacks
    {
        public event Action OnFightRoundFinished;
        public event Action OnEncounterFinished;
        public void TriggerRound();
    }
}