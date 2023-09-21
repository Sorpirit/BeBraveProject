using Library.GameFlow.StateSystem;

namespace Core.GameStates
{
    public interface IDropCard : IState
    {
        void DropCard(int i);
    }
}