using UnityEngine;

namespace UIBus
{
    public interface IColorSelection : IUIElement
    {
        void ShowColors(Renderer renderer);
    }
}