using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Jan.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonContainer : UIElement, IButtonContainer
    {
        [SerializeField] private Button button;
        
        public void RegisterButton(UnityAction action)
        {
            button.onClick.AddListener(action);
        }

        public void RemoveAllListeners()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}