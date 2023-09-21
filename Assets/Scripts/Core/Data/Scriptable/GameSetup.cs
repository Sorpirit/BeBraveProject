using Core.RoomsSystem;

namespace Core.Data.Scriptable
{
    public class GameSetup
    {
        public CardSet CardSet { get; set; }
        public IRoomFactory RoomFactory { get; set; }
        public int MaxCardsInHand { get; set; }
        public int MaxPlayerHealth { get; set; }
    }
}