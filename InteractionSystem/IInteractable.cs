namespace Jan.InteractionSystem
{
    public interface IInteractable
    {
        bool HighlightEffect { get; }
        bool IsHoldable { get; }
        string Tooltip { get; }

        void Interact(int buttonIndex);
    }
}

