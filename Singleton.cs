using UnityEngine;
using Sirenix.OdinInspector;

namespace Jan.Core
{
    /// <summary>
    /// A base generic class implementing the Singleton design pattern for MonoBehaviours.
    /// </summary>
    /// <typeparam name="T">The type of the Singleton class inheriting from this base class.</typeparam>
    /// <remarks>
    /// This class ensures a single instance of the inheriting class across the application.
    /// If multiple instances are detected in the scene, a debug error message is logged.
    /// If no instance exists, the singleton instance will remain null.
    /// </remarks>
    /// <example>
    /// To use this class, derive from it and pass your class type as the generic type parameter.
    /// Public static methods or properties can then access the instance.
    /// </example>
    [DefaultExecutionOrder(-5)]
    public abstract class Singleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
    {
        /// <summary>
        /// Provides global access to the singleton instance of a class that inherits from the `Singleton<T>` base class.
        /// Automatically finds and assigns the instance from the scene if not already initialized.
        /// Logs warnings if multiple instances are detected or if no instance is found in the scene.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (FindObjectsByType(typeof(T), FindObjectsSortMode.None) is T[] { Length: > 0 } objs)
                    {
                        _instance = objs[0];
                        if (objs.Length > 1) Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                    }

                    //if (_instance == null) Debug.Log($"There is no any {typeof(T).Name}");
                }

                return _instance;
            }
        }

        /// <summary>
        /// Holds the instance of the singleton of type <see cref="T"/>.
        /// Ensures only one instance exists within the scene, and provides global access to it.
        /// </summary>
        /// <remarks>
        /// This variable is internally used to store the single instance of the type <see cref="T"/>.
        /// If no instance is found in the scene, a debug message will indicate this.
        /// If multiple instances are present, an error message will be logged to notify of the conflict.
        /// </remarks>
        private static T _instance;
    }
}