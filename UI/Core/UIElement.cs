using Jan.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jan.UI
{
    public class UIElement : MonoBehaviour, IUIElement
    {
        [Header("UI Settings"), FoldoutGroup("UI Element")]
        [SerializeField, FoldoutGroup("UI Element")] protected GameObject ui;
        [SerializeField, ValueDropdown(nameof(GetUINames)), FoldoutGroup("UI Element")] private string _key = UINames.None;
        private string[] GetUINames => GlobalsUtils.GetNames(typeof(UINames));
        [SerializeField, FoldoutGroup("UI Element")] private bool pauseGame;

        protected virtual void Awake()
        {
            UIBusManager.RegisterUIElement(_key, this);
        }

        protected virtual void OnDestroy()
        {
            UIBusManager.UnregisterUIElement(_key, this);
        }

        public virtual void Show(bool show)
        {
            ui.SetActive(show);

            if (show && pauseGame)
            {
                Time.timeScale = 0f;
                GameStateManager.SetGameState(GameState.UI);
            }
            else
            {
                Time.timeScale = 1f;

                if(!pauseGame) return;
                if (GameStateManager.CurrentGameState == GameState.UI)
                {
                    GameStateManager.SetGameState(GameStateManager.PreviousGameState);
                }
            }
        }
    }
}