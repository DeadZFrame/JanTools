using Jan.UI;
using TMPro;
using UnityEngine;

public class Tooltip : UIElement, ITooltip
{
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private Vector2 offset;

    public void SetTooltip(string text, Vector2 position)
    {
        tooltipText.text = text;
        transform.position = position + offset;
    }
}
