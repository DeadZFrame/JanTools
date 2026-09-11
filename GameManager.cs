using Jan.Events;
using Steamworks;
using UnityEngine;

namespace Jan.Pool
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private bool enableSteamDRM = true;

        void Awake()
        {
            if (enableSteamDRM && SteamAPI.RestartAppIfNecessary((AppId_t)5245040)) 
            {
                Application.Quit();
                return;
            }
        }
        
        void OnDisable()
        {
            JanPool.Dispose();
            EventManager.Dispose();
        }
    }
}