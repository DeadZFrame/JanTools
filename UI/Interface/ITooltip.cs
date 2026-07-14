using Jan.UI;
using UnityEngine;

public interface ITooltip : IUIElement
{
    void SetTooltip(string text, Vector2 position);
}
