using TMPro;
using UnityEngine;

namespace Jan.UI
{
    public class TextContainer : UIElement
    {
        [SerializeField] private TextMeshProUGUI textMesh;

        public virtual void SetText(string text)
        {
            textMesh.SetText(text);
        }
    }
}