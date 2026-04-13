namespace Jan.InteractionSystem
{
    public interface IInteractable
    {
        bool HighlightEffect { get; }
        bool IsHoldable { get; }
        string Tooltip { get; }

        void OnHover();

        void HoverOut();

        void Interact(int buttonIndex);
    }
}

