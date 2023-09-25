namespace Core.CardSystem.Data.CardDescriptors
{
    public class EmptyDescription : ICardDescription
    {
        public static readonly EmptyDescription Instance = new (); 
        
        private EmptyDescription() { }
    }
}