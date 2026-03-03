using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Jan.Tasks
{
    /// <summary>
    /// The Timed class provides utilities for scheduling periodic, delayed, or continuous actions
    /// with various timing options, including support for unscaled time, player loop timing, and
    /// object-based culling conditions.
    /// </summary>
    public static partial class Timed
    {
        #region CallPeriodically

        /// <summary>
        /// Repeatedly executes a specified action at given time intervals for the specified duration.
        /// Execution is tied to the provided GameObject, and will stop if the GameObject is destroyed or culled.
        /// </summary>
        /// <param name="duration">The total duration (in seconds) for which the action should be executed.</param>
        /// <param name="timeStep">The interval (in seconds) between each execution of the action.</param>
        /// <param name="action">The action to be executed periodically.</param>
        /// <param name="cullingObject">The GameObject whose existence determines if the action should continue. If the GameObject is destroyed or inactive, the execution is stopped.</param>
        /// <returns>A CancellationTokenSource that can be used to manually stop the scheduled executions.</returns>
        public static Cts CallPeriodically(double duration, float timeStep, Action action, GameObject cullingObject)
        {
            return CallPeriodically(duration, timeStep, action, cullingObject, false);
        }

        /// <summary>
        /// Executes a specified action periodically for a given duration.
        /// </summary>
        /// <param name="duration">The total duration (in seconds) for which the action should be executed periodically.</param>
        /// <param name="timeStep">The time interval (in seconds) between each action execution.</param>
        /// <param name="action">The action to be executed periodically.</param>
        /// <returns>A <see cref="Cts"/> that can be used to cancel the periodic execution.</returns>
        public static Cts CallPeriodically(double duration, float timeStep, Action action)
        {
            return CallPeriodically(duration, timeStep, action, false);
        }

        /// <summary>
        /// Repeatedly invokes the specified action at regular intervals over the specified duration.
        /// </summary>
        /// <param name="duration">The duration for which the specified action should be invoked, in seconds.</param>
        /// <param name="timeStep">The time interval between each invocation of the specified action, in seconds.</param>
        /// <param name="action">The action to be invoked periodically.</param>
        /// <param name="unscaledTime">
        /// A boolean indicating whether to use unscaled time for interval calculations (true) or scaled time (false).
        /// </param>
        /// <returns>A <see cref="Cts"/> that can be used to cancel the periodic invocations.</returns>
        public static Cts CallPeriodically(double duration, float timeStep, Action action, bool unscaledTime)
        {
            return CallPeriodically(duration, timeStep, action, null, unscaledTime);
        }

        /// Calls the specified action periodically at fixed intervals for a given duration.
        /// <param name="duration">
        /// The total duration in seconds for which the periodic calls should be made.
        /// </param>
        /// <param name="timeStep">
        /// The interval in seconds between each call to the provided action.
        /// </param>
        /// <param name="action">
        /// The action to be executed periodically.
        /// </param>
        /// <param name="cullingObject">
        /// A GameObject used to determine if execution should be suspended. If this GameObject becomes null, periodic calls stop.
        /// </param>
        /// <param name="unscaledTime">
        /// A boolean indicating whether to use unscaled time for interval calculations (true) or scaled time (false).
        /// </param>
        /// <returns>
        /// A CancellationTokenSource that can be used to cancel the periodic execution.
        /// </returns>
        public static Cts CallPeriodically(double duration, float timeStep, Action action, GameObject cullingObject, bool unscaledTime)
        {
            return CallPeriodically(duration, timeStep, action, cullingObject, unscaledTime, PlayerLoops.Update);
        }

        /// Calls an action periodically within a specified duration, executing the action at defined intervals,
        /// and allowing control over the player loop timing to synchronize with Unity's player loop phases.
        /// <param name="duration">The total duration in seconds during which the action will be repeatedly invoked.</param>
        /// <param name="timeStep">The interval in seconds between each invocation of the action.</param>
        /// <param name="action">The action to be invoked periodically.</param>
        /// <param name="playerLoop">Specifies the player loop timing phase during which the action should be executed.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the periodic invocation early.</returns>
        public static Cts CallPeriodically(double duration, float timeStep, Action action, PlayerLoops playerLoop)
        {
            return CallPeriodically(duration, timeStep, action, false, playerLoop);
        }

        /// Calls a specified action periodically for a given duration, with a defined time step.
        /// Provides several overloads to customize behavior with optional parameters like culling objects, unscaled time, and player loop timing.
        /// <param name="duration">The total duration in seconds for which the periodic calls should occur.</param>
        /// <param name="timeStep">The time interval in seconds between each action execution.</param>
        /// <param name="action">The action to execute periodically.</param>
        /// <param name="unscaledTime">Indicates whether to use unscaled time for the periodic calls. Default is false.</param>
        /// <param name="playerLoop">Specifies the player loop timing phase during which the action should be processed (Unity-specific feature).</param>
        /// <return>A CancellationTokenSource that can be used to cancel the periodic calls before the total duration elapses.</return>
        public static Cts CallPeriodically(double duration, float timeStep, Action action, bool unscaledTime, PlayerLoops playerLoop)
        {
            return CallPeriodically(duration, timeStep, action, null, unscaledTime, playerLoop);
        }

        /// <summary>
        /// Calls the provided action periodically for the specified duration and time step.
        /// An optional culling object and player loop timing can be specified for controlling execution.
        /// </summary>
        /// <param name="duration">The total duration (in seconds) during which the action is periodically invoked.</param>
        /// <param name="timeStep">The interval (in seconds) between each invocation of the action.</param>
        /// <param name="action">The action to be invoked periodically.</param>
        /// <param name="cullingObject">An optional GameObject that, if not active, will stop the periodic invocation. Null if not used.</param>
        /// <param name="playerLoop">The timing in the player loop when the action should be invoked.</param>
        /// <returns>Returns a CancellationTokenSource that can be used to cancel the periodic invocation.</returns>
        public static Cts CallPeriodically(double duration, float timeStep, Action action, GameObject cullingObject, PlayerLoops playerLoop)
        {
            return CallPeriodically(duration, timeStep, action, cullingObject, false, playerLoop);
        }

        /// Calls a specified action periodically for a given duration with a defined time step.
        /// A cancellation token is returned, which allows canceling the periodic action if needed.
        /// Additionally, the method can integrate culling mechanisms based on the life of a game object
        /// or modify time measurement using unscaled time or specific player loop timing options.
        /// <param name="duration">The total duration in seconds for which the action should be called periodically.</param>
        /// <param name="timeStep">The interval in seconds between successive calls of the action.</param>
        /// <param name="action">The action to be executed periodically.</param>
        /// <param name="cullingObject">An optional game object whose destruction will cancel the periodic action.</param>
        /// <param name="unscaledTime">Whether to use unscaled time (true) or scaled time (false) for delay calculations.</param>
        /// <param name="playerLoop">Defines the point in Unity's player loop where the periodic updates will be executed.</param>
        /// <returns>A CancellationTokenSource to allow manual cancellation of the periodic action if necessary.</returns>
        public static Cts CallPeriodically(double duration, float timeStep, Action action, GameObject cullingObject, bool unscaledTime, PlayerLoops playerLoop)
        {
            var cts = CancellationTokenSourcePool.GetCtsFromPool(cullingObject);
            Task(cullingObject == null ? cts.Token : cts.LinkedTokenSource.Token).Forget();

            return cts;

            async UniTaskVoid Task(CancellationToken token)
            {
                var canceled = false;

                while (duration > 0f && !canceled)
                {
                    action?.Invoke();
                    canceled = await UniTask.Delay(TimeSpan.FromSeconds(timeStep), unscaledTime, (PlayerLoopTiming)playerLoop, token).TryAwait();
                    duration -= timeStep;
                }

                if(!canceled) cts?.CompletedCallback?.Invoke();
                else cts?.CancellationCallback?.Invoke();

                if(!canceled) CancellationTokenSourcePool.ReturnToPool(cts);
            }
        }

        #endregion

        #region CallDelayed

        /// Schedules an action to be executed after a specified delay.
        /// <param name="delay">The delay in seconds before the action is executed.</param>
        /// <param name="action">The action to execute after the delay.</param>
        /// <param name="cullingObject">The GameObject used for culling. If the object is destroyed before the delay is complete,
        /// the action will not be executed.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the scheduled action before execution.</returns>
        public static Cts CallDelayed(double delay, Action action, GameObject cullingObject)
        {
            return CallDelayed(delay, action, cullingObject, false);
        }

        /// <summary>
        /// Schedules the execution of a given action after a specified delay.
        /// </summary>
        /// <param name="delay">The delay in seconds before the action is executed.</param>
        /// <param name="action">The action to be executed after the delay.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the scheduled action.</returns>
        public static Cts CallDelayed(double delay, Action action)
        {
            return CallDelayed(delay, action, false);
        }

        /// <summary>
        /// Schedules an action to be executed after a specified delay.
        /// </summary>
        /// <param name="delay">The delay in seconds after which the action will be executed.</param>
        /// <param name="action">The action to execute after the delay.</param>
        /// <param name="unscaledTime">Indicates whether the delay should be affected by time scaling.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the execution of the action.</returns>
        public static Cts CallDelayed(double delay, Action action, bool unscaledTime)
        {
            return CallDelayed(delay, action, null, unscaledTime);
        }

        /// Schedules an action to be invoked after a specified delay.
        /// <param name="delay">The delay in seconds before the action is executed.</param>
        /// <param name="action">The action to execute after the delay.</param>
        /// <param name="cullingObject">An optional GameObject. If specified and the GameObject is null or destroyed, the action will not be executed.</param>
        /// <param name="unscaledTime">Determines whether the delay uses unscaled time. If true, the delay will be unaffected by time scaling (e.g., Time.timeScale).</param>
        /// <returns>Returns a CancellationTokenSource that can be used to cancel the scheduled action.</returns>
        public static Cts CallDelayed(double delay, Action action, GameObject cullingObject, bool unscaledTime)
        {
            return CallDelayed(delay, action, cullingObject, unscaledTime, PlayerLoops.Update);
        }

        /// <summary>
        /// Schedules a delayed action to be executed after the specified delay duration.
        /// </summary>
        /// <param name="delay">The delay duration in seconds before executing the action.</param>
        /// <param name="action">The action to be executed after the delay.</param>
        /// <param name="playerLoop">The player loop timing at which the action will be executed.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the delayed action.</returns>
        public static Cts CallDelayed(double delay, Action action, PlayerLoops playerLoop)
        {
            return CallDelayed(delay, action, null, playerLoop);
        }

        /// Schedules the execution of an action to occur after a specified delay.
        /// The execution can be optionally tied to a specified GameObject,
        /// unscaled time, or executed within a specified PlayerLoops.
        /// <param name="delay">The delay in seconds before the action is executed.</param>
        /// <param name="action">The action to be executed after the delay.</param>
        /// <param name="cullingObject">The GameObject used for culling. Execution is canceled if this GameObject is destroyed. This parameter can be null.</param>
        /// <param name="playerLoop">The PlayerLoops stage at which the action should be executed.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the scheduled execution.</returns>
        public static Cts CallDelayed(double delay, Action action, GameObject cullingObject, PlayerLoops playerLoop)
        {
            return CallDelayed(delay, action, cullingObject, false, playerLoop);
        }

        /// Schedules the provided action to be executed after a specified delay. This method allows
        /// for customization using parameters like unscaled time, culling object, or player loop timing.
        /// A CancellationTokenSource is returned to allow cancellation of the delayed action.
        /// <param name="delay">The delay in seconds before the action is executed.</param>
        /// <param name="action">The Action delegate representing the method to be executed after the delay.</param>
        /// <param name="unscaledTime">If true, the delay will use unscaled time, ignoring time scale modifications.</param>
        /// <param name="playerLoop">Specifies the PlayerLoops for when the action will execute within Unity's loop cycle.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the scheduled action before execution.</returns>
        public static Cts CallDelayed(double delay, Action action, bool unscaledTime, PlayerLoops playerLoop)
        {
            return CallDelayed(delay, action, null, unscaledTime, playerLoop);
        }

        /// Schedules an action to be executed after a specified delay. Supports optional cancellation and unscaled time mode.
        /// <param name="delay">The delay in seconds before the action is executed.</param>
        /// <param name="action">The action to execute after the delay.</param>
        /// <param name="cullingObject">An optional GameObject to monitor for cancellation. The action will not execute if the object is destroyed.</param>
        /// <param name="unscaledTime">Determines if the delay uses real time (when true) or scaled time (when false).</param>
        /// <param name="playerLoop">Defines the player loop timing at which the action is executed.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the scheduled action.
        /// After the action has been executed or canceled, the CancellationTokenSource will be returned to an internal pool for reuse.
        /// </returns>
        public static Cts CallDelayed(double delay, Action action, GameObject cullingObject, bool unscaledTime, PlayerLoops playerLoop)
        {
            var cts = CancellationTokenSourcePool.GetCtsFromPool(cullingObject);
            Task(cullingObject == null ? cts.Token : cts.LinkedTokenSource.Token).Forget();
            
            return cts;

            async UniTaskVoid Task(CancellationToken token)
            {
                var canceled = await UniTask.Delay(TimeSpan.FromSeconds(delay), unscaledTime, (PlayerLoopTiming)playerLoop, token).TryAwait();
                if (!canceled)
                {
                    action?.Invoke();
                    cts?.CompletedCallback?.Invoke();
                }
                else
                {
                    cts?.CancellationCallback?.Invoke();
                }

                if (!canceled) CancellationTokenSourcePool.ReturnToPool(cts);
            }
        }

        #endregion

        #region CallDelayedFrame

        /// <summary>
        /// Executes an action after a specific number of frames has passed.
        /// </summary>
        /// <param name="delay">The number of frames to wait before executing the action.</param>
        /// <param name="action">The action to execute after the delay.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the delayed action.</returns>
        public static Cts CallDelayedFrame(int delay, Action action)
        {
            return CallDelayedFrame(delay, action, null);
        }

        /// <summary>
        /// Executes a specified action after a set number of frames delay. If a culling object is provided, the operation can be conditionally executed based on the object's state.
        /// </summary>
        /// <param name="delay">The number of frames to delay the execution.</param>
        /// <param name="action">The action to execute after the delay.</param>
        /// <param name="cullingObject">An optional GameObject that determines if the action should be executed (e.g., based on its active state).</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the delayed execution.</returns>
        public static Cts CallDelayedFrame(int delay, Action action, GameObject cullingObject)
        {
            return CallDelayedFrame(delay, action, cullingObject, PlayerLoops.Update);
        }

        /// Executes the specified action after the defined number of frames, utilizing the specified player loop timing.
        /// <param name="delay">The number of frames to wait before executing the action.</param>
        /// <param name="action">The action to execute after the delay.</param>
        /// <param name="playerLoop">The player loop timing indicating when the action should be executed.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the delayed execution.</returns>
        public static Cts CallDelayedFrame(int delay, Action action, PlayerLoops playerLoop)
        {
            return CallDelayedFrame(delay, action, null, playerLoop);
        }

        /// <summary>
        /// Executes the specified action after a specific number of frames, considering optional culling and player loop timing settings.
        /// </summary>
        /// <param name="delay">The number of frames to delay before executing the action.</param>
        /// <param name="action">The action to execute after the delay.</param>
        /// <param name="cullingObject">The GameObject used to control the cancellation if destroyed. Can be null.</param>
        /// <param name="playerLoop">The timing phase in the Unity player loop where the delay will be executed.</param>
        /// <returns>Returns an instance of <see cref="Cts"/> that can be used to cancel the scheduled action.</returns>
        public static Cts CallDelayedFrame(int delay, Action action, GameObject cullingObject, PlayerLoops playerLoop)
        {
            var cts = CancellationTokenSourcePool.GetCtsFromPool(cullingObject);
            Task(cullingObject == null ? cts.Token : cts.LinkedTokenSource.Token).Forget();
            
            return cts;

            async UniTaskVoid Task(CancellationToken token)
            {
                bool canceled;
                canceled = await UniTask.DelayFrame(delay, (PlayerLoopTiming)playerLoop, token).TryAwait();

                if (!canceled)
                {
                    action?.Invoke();
                    cts?.CompletedCallback?.Invoke();
                }
                else
                {
                    cts?.CancellationCallback?.Invoke();
                }

                if (!canceled) CancellationTokenSourcePool.ReturnToPool(cts);
            }
        }

        #endregion
        
        #region Delay

        /// <summary>
        /// Delays execution for a specified amount of time.
        /// </summary>
        /// <param name="delay">The time, in seconds, to delay execution.</param>
        /// <returns>A <see cref="Cts"/> that can be used to cancel the delay.</returns>
        public static (Cts cts, UniTask task) Delay(float delay)
        {
            return Delay(delay, null);
        }

        /// Delays execution for a specified duration.
        /// <param name="delay">The duration of the delay, in seconds.</param>
        /// <param name="cullingObject">An optional GameObject to monitor. The operation will be canceled if this object is destroyed.</param>
        /// <returns>A CancellationTokenSource allowing for cancellation of the delay operation. The associated token can be used to await the delay asynchronously.</returns>
        public static (Cts cts, UniTask task) Delay(float delay, GameObject cullingObject)
        {
            return Delay(delay, cullingObject, false);
        }

        /// Creates a delay before executing the provided action or triggering a cancellation token.
        /// <param name="delay">The amount of time to delay in seconds.</param>
        /// <param name="unscaledTime">Specifies whether the delay should be affected by time scaling (if set to false) or use unscaled time (if set to true).</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the delay or check its status.</returns>
        public static (Cts cts, UniTask task) Delay(float delay, bool unscaledTime)
        {
            return Delay(delay, null, unscaledTime);
        }

        /// Delays the execution of operations for the specified duration.
        /// <param name="delay">The delay duration in seconds.</param>
        /// <param name="cullingObject">The GameObject used to check if the it should be culled. If the GameObject is null or destroyed during the delay, the operation will cancel.</param>
        /// <param name="unscaledTime">Specifies whether to use unscaled time instead of scaled time for the delay.</param>
        /// <returns>A CancellationTokenSource that allows the delayed operation to be controlled or canceled.</returns>
        public static (Cts cts, UniTask task) Delay(float delay, GameObject cullingObject, bool unscaledTime)
        {
            return Delay(delay, cullingObject, unscaledTime, PlayerLoops.Update);
        }

        /// <summary>
        /// Creates a delayed task that waits for the specified duration in seconds.
        /// </summary>
        /// <param name="delay">The delay duration in seconds before the task is executed.</param>
        /// <param name="playerLoop">
        /// The timing in the Unity player loop where the delay and subsequent task execution should occur.
        /// </param>
        /// <returns>
        /// A <see cref="Cts"/> that can be used to cancel the delayed task.
        /// </returns>
        public static (Cts cts, UniTask task) Delay(float delay, PlayerLoops playerLoop)
        {
            return Delay(delay, null, playerLoop);
        }

        /// <summary>
        /// Delays execution for a specified duration.
        /// </summary>
        /// <param name="delay">The delay duration in seconds.</param>
        /// <param name="playerLoop">Specifies the player loop timing at which the delay should be executed.</param>
        /// <param name="cullingObject">The GameObject used to check if the it should be culled. If the GameObject is null or destroyed during the delay, the operation will cancel.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the delay.</returns>
        public static (Cts cts, UniTask task) Delay(float delay, GameObject cullingObject, PlayerLoops playerLoop)
        {
            return Delay(delay, cullingObject, false, playerLoop);
        }

        /// <summary>
        /// Creates a delay that executes asynchronously for the specified duration.
        /// </summary>
        /// <param name="delay">The duration in seconds to wait before completion.</param>
        /// <param name="unscaledTime">Indicates if the delay should be based on unscaled time. If true, it is independent of time scaling (e.g., pause).</param>
        /// <param name="playerLoop">Specifies the player loop timing at which the delay will be updated.</param>
        /// <returns>Returns a <see cref="Cts"/> that can be used to cancel the delay.</returns>
        public static (Cts cts, UniTask task) Delay(float delay, bool unscaledTime, PlayerLoops playerLoop)
        {
            return Delay(delay, null, unscaledTime, playerLoop);
        }

        /// <summary>
        /// Delays the execution for a specified amount of time and returns a cancellation token source to manage the delay.
        /// </summary>
        /// <param name="delay">The duration of the delay in seconds.</param>
        /// <param name="cullingObject">
        /// The GameObject to monitor for cancellation. The delay will be automatically canceled if the specified GameObject
        /// is destroyed during the delay period. Can be null for no culling.
        /// </param>
        /// <param name="unscaledTime">
        /// Determines whether the delay duration is based on unscaled time (true) or scaled time (false).
        /// </param>
        /// <param name="playerLoop">
        /// The PlayerLoops at which the delay operation executes, representing a specific point in Unity's update loop.
        /// </param>
        /// <returns>
        /// A <see cref="Cts"/> instance that can be used to manually cancel the delay or to determine its completion state.
        /// </returns>
        public static (Cts cts, UniTask task) Delay(float delay, GameObject cullingObject, bool unscaledTime, PlayerLoops playerLoop)
        {
            var cts = CancellationTokenSourcePool.GetCtsFromPool(cullingObject);
            var task = Task(cullingObject == null ? cts.Token : cts.LinkedTokenSource.Token);

            return (cts, task);

            async UniTask Task(CancellationToken token)
            {
                var canceled = await UniTask.Delay(TimeSpan.FromSeconds(delay), unscaledTime, (PlayerLoopTiming)playerLoop, token).TryAwait();
                if(!canceled) cts?.CompletedCallback?.Invoke();
                else cts?.CancellationCallback?.Invoke();
                if(!canceled) CancellationTokenSourcePool.ReturnToPool(cts);
            }
        }

        #endregion

        #region CallContinuesFor

        /// <summary>
        /// Executes an action repeatedly for a specified duration.
        /// </summary>
        /// <param name="duration">The total time in seconds during which the action will be executed.</param>
        /// <param name="action">The action to be executed repeatedly.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the execution if needed.</param>
        public static Cts CallContinuesFor(double duration, Action action)
        {
            return CallContinuesFor(duration, action, null);
        }

        /// Initiates a periodic call of the specified action for the given duration, with optional parameters to control behavior.
        /// <param name="duration">
        /// The duration, in seconds, for which the action should continue to be called periodically.
        /// </param>
        /// <param name="action">
        /// The action to be executed periodically during the specified duration.
        /// </param>
        /// <param name="cullingObject">
        /// An optional GameObject parameter. If specified, the periodic calls will stop if this GameObject is destroyed.
        /// </param>
        /// <return>
        /// A CancellationTokenSource object that allows for manual cancellation of the periodic calls.
        /// </return>
        public static Cts CallContinuesFor(double duration, Action action, GameObject cullingObject)
        {
            return CallContinuesFor(duration, action, cullingObject, false);
        }

        /// Executes a specified action repeatedly for a given duration, providing the ability to use unscaled time.
        /// <param name="duration">The amount of time, in seconds, the action should continue being called for.</param>
        /// <param name="action">The action to execute repeatedly for the given duration.</param>
        /// <param name="unscaledTime">Determines whether the time should be based on unscaled time (e.g., ignoring in-game time scaling).</param>
        /// <returns>A CancellationTokenSource that can be used to stop the execution or check its status.</returns>
        public static Cts CallContinuesFor(double duration, Action action, bool unscaledTime)
        {
            return CallContinuesFor(duration, action, null, unscaledTime);
        }

        /// Schedules a periodic action to be invoked at regular intervals for a specified duration,
        /// with optional conditions for culling, unscaled time, and a specific Unity player loop timing stage.
        /// <param name="duration">The total time in seconds the action should be executed for.</param>
        /// <param name="action">The action to be invoked periodically during the specified duration.</param>
        /// <param name="cullingObject">
        /// A GameObject that, when not active or destroyed, will stop the periodic action. Pass null if no culling object is needed.
        /// </param>
        /// <param name="unscaledTime">
        /// A boolean flag indicating whether to use unscaled time (true) or scaled time (false) for timing calculations.
        /// </param>
        /// <returns>A cancellation token source allowing the periodic action to be stopped or canceled prematurely.</returns>
        public static Cts CallContinuesFor(double duration, Action action, GameObject cullingObject, bool unscaledTime)
        {
            return CallContinuesFor(duration, action, cullingObject, unscaledTime, PlayerLoops.Update);
        }

        /// <summary>
        /// Invokes a specified action continuously for a given duration at the specified PlayerLoops.
        /// </summary>
        /// <param name="duration">The time in seconds for which the action will be invoked continuously.</param>
        /// <param name="action">The action to be executed repeatedly during the specified duration.</param>
        /// <param name="playerLoop">The player loop timing at which to invoke the action.</param>
        /// <returns>A <see cref="CancellationTokenSource"/> used to manage and cancel the repeated invocation.</returns>
        public static Cts CallContinuesFor(double duration, Action action, PlayerLoops playerLoop)
        {
            return CallContinuesFor(duration, action, null, playerLoop);
        }

        /// Calls an action periodically for a specified duration using the provided player loop timing.
        /// <param name="duration">The total duration for which the action is to be invoked, in seconds.</param>
        /// <param name="action">The method to execute periodically while the duration has not elapsed.</param>
        /// <param name="cullingObject">
        /// A GameObject that, when not active or destroyed, will stop the periodic action. Pass null if no culling object is needed.
        /// </param>
        /// <param name="playerLoop">The Unity player loop timing at which the periodic calls will be made.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the periodic calls manually before the duration ends.</returns>
        public static Cts CallContinuesFor(double duration, Action action, GameObject cullingObject, PlayerLoops playerLoop)
        {
            return CallContinuesFor(duration, action, cullingObject, false, playerLoop);
        }

        /// <summary>
        /// Repeatedly invokes the specified action for a certain duration, with the ability to use unscaled time
        /// and customize the timing based on the Unity Player Loop system.
        /// </summary>
        /// <param name="duration">The total duration for which the action should be executed.</param>
        /// <param name="action">The action to be executed repeatedly.</param>
        /// <param name="unscaledTime">Determines if unscaled time (ignores time scale) should be used.</param>
        /// <param name="playerLoop">Specifies the Player Loop timing phase when the action should be executed.</param>
        /// <returns>A <see cref="Cts"/> that can be used to stop the execution prematurely.</returns>
        public static Cts CallContinuesFor(double duration, Action action, bool unscaledTime, PlayerLoops playerLoop)
        {
            return CallContinuesFor(duration, action, null, unscaledTime, playerLoop);
        }

        /// Runs the specified action repeatedly for the given duration.
        /// The action is executed within a defined player loop timing and can be controlled with unscaled time.
        /// Optionally, action execution can depend on a culling object's lifecycle.
        /// A cancellation token is returned, which combines with the internal logic to support cancellation or termination when certain conditions are met.
        /// <param name="duration">The total time in seconds for which the action will continue to run.</param>
        /// <param name="action">The action to execute repeatedly.</param>
        /// <param name="cullingObject">An optional GameObject used to determine when to stop the repeated action (e.g., if the object is destroyed).</param>
        /// <param name="unscaledTime">Indicates whether the execution intervals use unscaled time.</param>
        /// <param name="playerLoop">The specific player loop timing when the action will be executed.</param>
        /// <returns>
        /// A CancellationTokenSource that allows the operation to be canceled externally.
        /// </returns>
        public static Cts CallContinuesFor(double duration, Action action, GameObject cullingObject, bool unscaledTime, PlayerLoops playerLoop)
        {
            var cts = CancellationTokenSourcePool.GetCtsFromPool(cullingObject);
            Task(cullingObject == null ? cts.Token : cts.LinkedTokenSource.Token).Forget();

            return cts;

            async UniTaskVoid Task(CancellationToken token)
            {
                var canceled = false;

                while (duration > 0 && !canceled)
                {
                    action?.Invoke();

                    if (unscaledTime) duration -= Time.unscaledDeltaTime;
                    else duration -= Time.deltaTime;

                    canceled = await UniTask.Yield((PlayerLoopTiming)playerLoop, token).TryAwait();
                }

                if(!canceled) cts?.CompletedCallback?.Invoke();
                else cts?.CancellationCallback?.Invoke();

                if(!canceled) CancellationTokenSourcePool.ReturnToPool(cts);
            }
        }

        #endregion

        #region CallWhileTrue

        /// Repeatedly executes the specified action while the provided condition evaluates to true. The condition is checked
        /// before each action invocation. The method returns a CancellationTokenSource, which can be used to manually stop the execution.
        /// <param name="condition">A function that evaluates to a boolean indicating whether the action should continue to be executed.</param>
        /// <param name="action">The action to execute while the condition remains true.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the execution of the action.</returns>
        public static Cts CallWhileTrue(Func<bool> condition, Action action)
        {
            return CallWhileTrue(condition, action, null);
        }

        /// Invokes an action repeatedly while a specific condition evaluates to true, supporting optional culling behavior using a GameObject.
        /// The method continuously runs the given action as long as the condition delegate returns true. The operation will cease
        /// if the condition becomes false or if the associated GameObject is destroyed. A CancellationTokenSource is returned to allow manual cancellation of the operation.
        /// <param name="condition">A delegate that evaluates whether the repeated action should continue executing. The loop will terminate if this condition returns false.</param>
        /// <param name="action">An action to be executed while the condition delegate returns true.</param>
        /// <param name="cullingObject">A GameObject used to determine when to automatically cancel the operation. If the object is destroyed, the action will cease execution.</param>
        /// <returns>A CancellationTokenSource that allows manual cancellation of the operation.</returns>
        public static Cts CallWhileTrue(Func<bool> condition, Action action, GameObject cullingObject)
        {
            return CallWhileTrue(condition, action, cullingObject, PlayerLoops.Update);
        }

        /// Continuously executes the specified action while the given condition evaluates to true.
        /// <param name="condition">
        /// A function that returns a boolean value. The action will repeatedly execute as long as this function returns true.
        /// </param>
        /// <param name="action">
        /// The action to be executed while the condition evaluates to true.
        /// </param>
        /// <param name="playerLoop">
        /// Specifies the timing at which the action should be executed during the Unity player loop.
        /// </param>
        /// <returns>
        /// A CancellationTokenSource that can be used to cancel the execution of the action.
        /// </returns>
        public static Cts CallWhileTrue(Func<bool> condition, Action action, PlayerLoops playerLoop)
        {
            return CallWhileTrue(condition, action, null, playerLoop);
        }

        /// Executes a specified action repeatedly while a provided condition evaluates to true.
        /// This method creates an asynchronous task that runs in a given player loop timing. The task periodically checks
        /// the provided condition and invokes the specified action, continuing execution as long as the condition evaluates
        /// to true, the associated cancellation token is not canceled, and the culling object (if provided) has not been
        /// destroyed. Once any of these conditions fail, the task is disposed.
        /// Parameters:
        /// condition:
        /// A function representing the condition that determines whether the action should continue to be executed.
        /// This function should return `true` to continue execution or `false` to stop.
        /// action:
        /// The action to execute repeatedly while the condition evaluates to true.
        /// cullingObject:
        /// An optional GameObject used as a culling reference. The execution is stopped if this object gets destroyed.
        /// PlayerLoops:
        /// The `PlayerLoops` value which specifies the timing at which the repeated action will be executed within
        /// the Unity player loop.
        /// Returns:
        /// A `CancellationTokenSource` that can be used to cancel the repeating task manually if needed. Disposing
        /// of this token source will also stop execution of the action.
        public static Cts CallWhileTrue(Func<bool> condition, Action action, GameObject cullingObject, PlayerLoops playerLoop)
        {
            var cts = CancellationTokenSourcePool.GetCtsFromPool(cullingObject);
            Task(cullingObject == null ? cts.Token : cts.LinkedTokenSource.Token).Forget();

            return cts;

            async UniTaskVoid Task(CancellationToken token)
            {
                var canceled = false;

                while (condition() && !canceled)
                {
                    action?.Invoke();
                    canceled = await UniTask.Yield((PlayerLoopTiming)playerLoop, token).TryAwait();
                }

                if(!canceled) cts?.CompletedCallback?.Invoke();
                else cts?.CancellationCallback?.Invoke();

                if(!canceled) CancellationTokenSourcePool.ReturnToPool(cts);
            }
        }

        /// <summary>
        /// Periodically calls the given action while the specified condition remains true.
        /// Allows specifying time step, time scaling, culling object, and player loop timing.
        /// </summary>
        /// <param name="condition">A function that returns a boolean value indicating whether the action should continue to be called.</param>
        /// <param name="timeStep">The interval in seconds between successive calls to the action.</param>
        /// <param name="action">The action to be executed periodically.</param>
        /// <param name="ignoreTimeScale">If set to true, the timing will ignore time scaling (e.g., Unity's Time.timeScale).</param>
        /// <param name="cullingObject">A GameObject used to determine culling. If the object is destroyed, the periodic action will stop.</param>
        /// <returns>A CancellationTokenSource that can be used to stop the periodic action manually.</returns>
        public static Cts CallWhileTruePeriodically(Func<bool> condition, float timeStep, Action action, bool ignoreTimeScale, GameObject cullingObject)
        {
            return CallWhileTruePeriodically(condition, timeStep, action, ignoreTimeScale, cullingObject, PlayerLoops.Update);
        }

        /// Executes a given action periodically while a specified condition remains true.
        /// This method periodically executes the provided action as long as the condition evaluates to true.
        /// The interval between executions is determined by the specified timeStep. The timing behavior can be configured
        /// to either follow the normal time scale or ignore it, based on the overload used.
        /// Parameters:
        /// condition:
        /// A delegate (Func<bool>) representing the condition. The action is executed as long as this condition returns true.
        /// timeStep:
        /// The interval, in seconds, between each execution of the action.
        /// action:
        /// The action to be executed periodically while the condition is true.
        /// Returns:
        /// A CancellationTokenSource that can be used to cancel the periodic execution.
        public static Cts CallWhileTruePeriodically(Func<bool> condition, float timeStep, Action action)
        {
            return CallWhileTruePeriodically(condition, timeStep, action, false);
        }

        /// Repeatedly invokes the specified action at the given time intervals as long as the provided condition evaluates to true.
        /// The method will stop executing the action when the condition returns false or when the returned CancellationTokenSource is canceled.
        /// Parameters:
        /// condition:
        /// A function that returns a boolean value determining whether to continue invoking the action.
        /// timeStep:
        /// The interval in seconds between successive invocations of the action.
        /// action:
        /// The action to invoke periodically while the condition evaluates to true.
        /// Returns:
        /// A CancellationTokenSource that can be used to stop the periodic invocation of the action manually.
        public static Cts CallWhileTruePeriodically(Func<bool> condition, float timeStep, Action action, bool ignoreTimeScale)
        {
            return CallWhileTruePeriodically(condition, timeStep, action, ignoreTimeScale, PlayerLoops.Update);
        }

        /// Executes an action periodically while a specified condition is true.
        /// The method runs at specified intervals, optionally ignoring the time scale or depending on a given player loop timing routine.
        /// <param name="condition">The function that evaluates to true or false to determine whether to continue executing the action.</param>
        /// <param name="timeStep">The time interval, in seconds, between consecutive action executions.</param>
        /// <param name="action">The action to execute periodically while the condition is true.</param>
        /// <param name="ignoreTimeScale">Indicates whether the time scale should be ignored during action execution.</param>
        /// <param name="playerLoop">Specifies the player loop timing at which the action will be executed.</param>
        /// <returns>A CancellationTokenSource that allows the operation to be cancelled.</returns>
        public static Cts CallWhileTruePeriodically(Func<bool> condition, float timeStep, Action action, bool ignoreTimeScale, PlayerLoops playerLoop)
        {
            return CallWhileTruePeriodically(condition, timeStep, action, ignoreTimeScale, null, playerLoop);
        }

        /// Calls an action periodically while a specified condition evaluates to true.
        /// This method repeatedly executes the provided action at a defined time interval until the condition returns false.
        /// It operates on the specified player loop timing and supports an optional culling object for additional behavior control.
        /// Parameters:
        /// condition:
        /// A delegate representing the condition. The action is called periodically as long as this condition returns true.
        /// timeStep:
        /// The time interval in seconds between each execution of the provided action.
        /// action:
        /// The action to execute periodically while the condition is true.
        /// cullingObject:
        /// An optional GameObject that can be used to control execution. Execution may halt if the culling object is deactivated or destroyed.
        /// Returns:
        /// A CancellationTokenSource that can be used to cancel the periodic calls programmatically.
        public static Cts CallWhileTruePeriodically(Func<bool> condition, float timeStep, Action action, GameObject cullingObject)
        {
            return CallWhileTruePeriodically(condition, timeStep, action, cullingObject, PlayerLoops.Update);
        }

        /// Executes the specified action periodically as long as the given condition evaluates to true.
        /// A cancellation token is returned which can be used to cancel the periodic execution.
        /// <param name="condition">The condition that determines whether the action should continue being executed.</param>
        /// <param name="timeStep">The interval in seconds between consecutive executions of the action.</param>
        /// <param name="action">The action to be executed periodically.</param>
        /// <param name="playerLoop">The timing within the Unity player loop where the action execution is scheduled.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the periodic execution.</returns>
        public static Cts CallWhileTruePeriodically(Func<bool> condition, float timeStep, Action action, PlayerLoops playerLoop)
        {
            return CallWhileTruePeriodically(condition, timeStep, action, null, playerLoop);
        }

        /// <summary>
        /// Repeatedly executes the specified action at the defined time intervals while the given condition remains true.
        /// </summary>
        /// <param name="condition">The predicate condition to evaluate. The periodic action continues while this returns true.</param>
        /// <param name="timeStep">The time interval, in seconds, between consecutive executions of the action.</param>
        /// <param name="action">The action to be performed periodically.</param>
        /// <param name="cullingObject">The optional GameObject used for culling. The action stops executing if this object is destroyed or becomes invalid.</param>
        /// <param name="playerLoop">Specifies the timing within the Unity Player Loop where the action is executed.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the periodic action.</returns>
        public static Cts CallWhileTruePeriodically(Func<bool> condition, float timeStep, Action action, GameObject cullingObject, PlayerLoops playerLoop)
        {
            return CallWhileTruePeriodically(condition, timeStep, action, false, cullingObject, playerLoop);
        }

        /// <summary>
        /// Executes the specified action periodically as long as the provided condition evaluates to true.
        /// Execution is tied to the given GameObject, and will stop if the GameObject is destroyed or culled.
        /// </summary>
        /// <param name="condition">A function that evaluates to true while the action should continue executing.</param>
        /// <param name="timeStep">The interval (in seconds) between each execution of the action.</param>
        /// <param name="action">The action to be executed periodically.</param>
        /// <param name="ignoreTimeScale">Specifies whether the action execution should ignore Unity's time scale setting.</param>
        /// <param name="cullingObject">The GameObject whose lifetime determines if the execution should continue. If the GameObject is destroyed or inactive, the execution stops.</param>
        /// <param name="playerLoop">The player loop timing stage during which the task should be executed.</param>
        /// <returns>A CancellationTokenSource that can be used to manually stop the execution.</returns>
        public static Cts CallWhileTruePeriodically(Func<bool> condition, float timeStep, Action action, bool ignoreTimeScale, GameObject cullingObject, PlayerLoops playerLoop)
        {
            var cts = CancellationTokenSourcePool.GetCtsFromPool(cullingObject);
            Task(cullingObject == null ? cts.Token : cts.LinkedTokenSource.Token).Forget();

            return cts;

            async UniTaskVoid Task(CancellationToken token)
            {
                var canceled = false;

                while (condition() && !canceled)
                {
                    action?.Invoke();
                    canceled = await UniTask.WaitForSeconds(timeStep, ignoreTimeScale, (PlayerLoopTiming)playerLoop, token).TryAwait();
                }

                if(!canceled) cts?.CompletedCallback?.Invoke();
                else cts?.CancellationCallback?.Invoke();

                if(!canceled) CancellationTokenSourcePool.ReturnToPool(cts);
            }
        }

        #endregion

        #region CallAfterTrue

        /// <summary>
        /// Repeatedly evaluates the specified condition and triggers the given action once the condition returns true.
        /// </summary>
        /// <param name="condition">A function that evaluates to a boolean value. The action is executed when this function returns true.</param>
        /// <param name="action">The action to execute when the condition evaluates to true.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the operation if needed.</returns>
        public static Cts CallAfterTrue(Func<bool> condition, Action action)
        {
            return CallAfterTrue(condition, action, null);
        }

        /// <summary>
        /// Repeatedly evaluates a given condition and invokes the specified action once the condition is met.
        /// The monitoring stops if the associated GameObject is destroyed.
        /// </summary>
        /// <param name="condition">A function that returns a boolean value indicating whether the condition is met.</param>
        /// <param name="action">The action to execute when the condition evaluates to true.</param>
        /// <param name="cullingObject">The GameObject used to stop execution when it is destroyed.</param>
        /// <returns>A CancellationTokenSource that can be used to manually cancel the execution.</returns>
        public static Cts CallAfterTrue(Func<bool> condition, Action action, GameObject cullingObject)
        {
            return CallAfterTrue(condition, action, cullingObject, PlayerLoops.Update);
        }

        /// <summary>
        /// Invokes the specified action repeatedly within the player loop once the given condition becomes true.
        /// </summary>
        /// <param name="condition">A function that evaluates to a boolean value; specifies the condition to check.</param>
        /// <param name="action">The action to invoke once the condition evaluates to true.</param>
        /// <param name="playerLoop">The timing within the Unity player loop where the action will be executed.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the scheduled invocation.</returns>
        public static Cts CallAfterTrue(Func<bool> condition, Action action, PlayerLoops playerLoop)
        {
            return CallAfterTrue(condition, action, null, playerLoop);
        }

        /// <summary>
        /// Continuously checks a given condition and invokes an action when the condition evaluates to true.
        /// Allows optional culling based on the lifecycle of a specified GameObject and control over execution timing.
        /// </summary>
        /// <param name="condition">The condition to evaluate repeatedly until it returns true.</param>
        /// <param name="action">The action to perform when the condition evaluates to true.</param>
        /// <param name="cullingObject">An optional GameObject whose lifecycle can terminate the operation if it is destroyed.</param>
        /// <param name="playerLoop">Defines the timing within the Unity player loop when the condition is evaluated.</param>
        /// <returns>A CancellationTokenSource that can be used to cancel the operation.</returns>
        public static Cts CallAfterTrue(Func<bool> condition, Action action, GameObject cullingObject, PlayerLoops playerLoop)
        {
            var cts = CancellationTokenSourcePool.GetCtsFromPool(cullingObject);
            Task(cullingObject == null ? cts.Token : cts.LinkedTokenSource.Token).Forget();

            return cts;

            async UniTaskVoid Task(CancellationToken token)
            {
                var canceled = await UniTask.WaitUntil(condition, (PlayerLoopTiming)playerLoop, token).TryAwait();
                if (!canceled)
                {
                    action?.Invoke();
                }

                if(!canceled) cts?.CompletedCallback?.Invoke();
                else cts?.CancellationCallback?.Invoke();
                
                if(!canceled) CancellationTokenSourcePool.ReturnToPool(cts);
            }
        }

        #endregion

        /// Releases all resources used by the Timed class and clears the internal cancellation token pool.
        /// This method iterates through the internal queue of CancellationTokenSource objects, cancels each token
        /// if it has not been already canceled, and then disposes of it. After disposing of all tokens, the queue
        /// is cleared to free memory and reset the state.
        /// This method is typically called during application shutdown or when the Timed functionality is no longer needed
        /// to ensure proper cleanup of resources.
        /// Exceptions:
        /// If a CancellationTokenSource cannot be cancelled or disposed of successfully, the method continues with
        /// the next token without throwing an exception, ensuring that all resources are released as intended.
        internal static void Dispose()
        {
            CancellationTokenSourcePool.Clear();
        }
    }
}