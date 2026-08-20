namespace Jan.UI
{
    public interface IUIElement
    {
        public bool IsActive { get; }
        void Show(bool show);
    }
}