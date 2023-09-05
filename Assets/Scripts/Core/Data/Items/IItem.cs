using Core.PlayerSystems;

namespace Core.Data.Items
{
    public interface IItem
    {
        void Use(PlayerPawn player);
    }
}