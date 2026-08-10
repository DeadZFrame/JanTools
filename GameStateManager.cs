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
        Any = ~0,
    }

    public enum SubStates
    {
        None,
        Painting,
        Cleaning,
        ObjectPlacement,
        Building,
        DryingBench,
    }

    public static class GameStateManager
    {
        public static GameState CurrentGameState { get; private set; }
        public static GameState PreviousGameState { get; private set; }
        public static SubStates CurrentSubState { get; private set; }

        public static void SetGameState(GameState newState, bool force = false)
        {
            if(newState == CurrentGameState && !force)
            {
                Debug.LogWarning($"Game State is already set to {newState}. No change made.");
                return;
            }
            
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

                default:
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
            }

            CurrentGameState = newState;
            EventManager.Trigger(EventNames.OnGameStateChanged, newState);

            Debug.Log($"Game State changed to: {newState}");
        }

        public static void SetSubState(SubStates newSubState)
        {
            CurrentSubState = newSubState;
            EventManager.Trigger(EventNames.OnGameStateChanged, newSubState);

            Debug.Log($"SubState changed to: {newSubState}");
        }
    }
}

