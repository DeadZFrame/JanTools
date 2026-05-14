using System;
using Jan.Events;
using UnityEngine;

namespace Jan.Core
{
    [Flags]
    public enum GameState
    {
        Paused = 1 << 0,
        Workshop = 1 << 1,
        FPS = 1 << 2,
        UI = 1 << 3,
        Build = 1 << 4,
        Any = ~0,
    }

    public static class GameStateManager
    {
        public static GameState CurrentGameState { get; private set; }
        public static GameState PreviousGameState { get; private set; }

        public static void SetGameState(GameState newState)
        {
            if(PreviousGameState != CurrentGameState) PreviousGameState = CurrentGameState;
            
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

                    case GameState.Build:
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                        break;

                default:
                    break;
            }

            CurrentGameState = newState;
            EventManager.Trigger<GameState>(EventNames.OnGameStateChanged, newState);

            Debug.Log($"Game State changed to: {newState}");
        }
    }
}

