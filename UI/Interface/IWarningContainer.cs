using Jan.UI;
using UnityEngine;

namespace UI
{
    public interface IWarningUI : IUIElement
    {
        void SetWarningText(string text, bool isWarning);
    }
}