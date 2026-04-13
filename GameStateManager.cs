using Jan.Events;
using UnityEngine;

namespace Jan.Core
{
    public enum GameState
    {
        Paused,
        Workshop,
        FPS,
        UI,
        Any,
    }

    public class GameStateManager : Singleton<GameStateManager>
    {
        public GameState CurrentGameState { get; private set; }

        public static void SetGameState(GameState newState)
        {
            Instance.CurrentGameState = newState;
            EventManager.Trigger<GameState>(EventNames.OnGameStateChanged, newState);

            switch (newState)
            {
                case GameState.Paused:
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                case GameState.UI:
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;

                    case GameState.FPS:
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;

                    case GameState.Workshop:
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;

                default:
                    break;
            }
        }
    }
}

