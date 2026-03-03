using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Jan.Tasks
{
    public static partial class Timed
    {
        /// <summary>
        /// Schedules an action to be executed after a specified delay (in seconds) on the calling MonoBehaviour.
        /// Uses scaled time and the default player loop phase (Update).
        /// </summary>
        /// <typeparam name="T">The MonoBehaviour type to associate with the delay and culling.</typeparam>
        /// <param name="obj">The MonoBehaviour instance to associate with the delay and culling.</param>
        /// <param name="delay">The delay in seconds before the action is executed.</param>
        /// <param name="action">The action to execute after the delay. The MonoBehaviour instance will be passed as a parameter.</param>
        /// <returns>A Cts that can be used to cancel the scheduled action.</returns>
        public static Cts CallDelayed<T>(this T obj, double delay, Action<T> action)
        {
            return CallDelayed(obj, delay, action, false, PlayerLoops.Update, null);
        }

        public static Cts CallDelayed<T>(this T obj, double delay, Action<T> action, GameObject cullingObject)
        {
            return CallDelayed(obj, delay, action, false, PlayerLoops.Update, cullingObject);
        }

        /// <summary>
        /// Schedules an action to be executed after a specified delay (in seconds) on the calling MonoBehaviour,
        /// at a specific Unity player loop phase. Uses scaled time.
        /// </summary>
        /// <typeparam name="T">The MonoBehaviour type to associate with the delay and culling.</typeparam>
        /// <param name="obj">The MonoBehaviour instance to associate with the delay and culling.</param>
        /// <param name="delay">The delay in seconds before the action is executed.</param>
        /// <param name="action">The action to execute after the delay. The MonoBehaviour instance will be passed as a parameter.</param>
        /// <param name="playerLoop">The Unity player loop phase at which the action is executed.</param>
        /// <returns>A Cts that can be used to cancel the scheduled action.</returns>
        public static Cts CallDelayed<T>(this T obj, double delay, Action<T> action, PlayerLoops playerLoop)
        {
            return CallDelayed(obj, delay, action, false, playerLoop, null);
        }

        /// <summary>
        /// Schedules an action to be executed after a specified delay (in seconds) on the calling MonoBehaviour,
        /// optionally using unscaled time. Uses the default player loop phase (Update).
        /// </summary>
        /// <typeparam name="T">The MonoBehaviour type to associate with the delay and culling.</typeparam>
        /// <param name="obj">The MonoBehaviour instance to associate with the delay and culling.</param>
        /// <param name="delay">The delay in seconds before the action is executed.</param>
        /// <param name="action">The action to execute after the delay. The MonoBehaviour instance will be passed as a parameter.</param>
        /// <param name="unscaledTime">If true, uses unscaled time for the delay; otherwise, uses scaled time.</param>
        /// <returns>A Cts that can be used to cancel the scheduled action.</returns>
        public static Cts CallDelayed<T>(this T obj, double delay, Action<T> action, bool unscaledTime)
        {
            return CallDelayed(obj, delay, action, unscaledTime, PlayerLoops.Update, null);
        }

        public static Cts CallDelayed<T>(this T obj, double delay, Action<T> action, bool unscaledTime, PlayerLoops playerLoop)
        {
            return CallDelayed(obj, delay, action, unscaledTime, playerLoop, null);
        }

        public static Cts CallDelayed<T>(this T obj, double delay, Action<T> action, bool unscaledTime, GameObject cullingObject)
        {
            return CallDelayed(obj, delay, action, unscaledTime, PlayerLoops.Update, cullingObject);
        }
        
        public static Cts CallDelayed<T>(this T obj, double delay, Action<T> action, PlayerLoops playerLoop, GameObject cullingObject)
        {
            return CallDelayed(obj, delay, action, false, playerLoop, cullingObject);
        }

        /// <summary>
        /// Schedules an action to be executed after a specified delay (in seconds) on the calling MonoBehaviour,
        /// optionally using unscaled time and specifying the Unity player loop phase.
        /// Uses the calling MonoBehaviour's GameObject as the culling object. This method also manages cancellation and completion callbacks via a pooled CancellationTokenSource.
        /// </summary>
        /// <typeparam name="T">The MonoBehaviour type to associate with the delay and culling.</typeparam>
        /// <param name="obj">The MonoBehaviour instance to associate with the delay and culling.</param>
        /// <param name="delay">The delay in seconds before the action is executed.</param>
        /// <param name="action">The action to execute after the delay. The MonoBehaviour instance will be passed as a parameter.</param>
        /// <param name="unscaledTime">If true, uses unscaled time for the delay; otherwise, uses scaled time.</param>
        /// <param name="playerLoop">The Unity player loop phase at which the action is executed.</param>
        /// <returns>A Cts that can be used to cancel the scheduled action. After the action has been executed or canceled, the CancellationTokenSource will be returned to an internal pool for reuse.</returns>
        public static Cts CallDelayed<T>(this T obj, double delay, Action<T> action, bool unscaledTime, PlayerLoops playerLoop, GameObject cullingObject)
        {
            Component mono = obj as Component;
            if(mono != null && cullingObject == null) cullingObject = mono.gameObject;

            var cts = CancellationTokenSourcePool.GetCtsFromPool(cullingObject);
            Task(cullingObject == null ? cts.Token : cts.LinkedTokenSource.Token, obj).Forget();

            return cts;

            async UniTaskVoid Task(CancellationToken token, T component)
            {
                var canceled = await UniTask.Delay(TimeSpan.FromSeconds(delay), unscaledTime, (PlayerLoopTiming)playerLoop, token).TryAwait();
                if (!canceled)
                {
                    action?.Invoke(component);
                    cts?.CompletedCallback?.Invoke();
                }
                else
                {
                    cts?.CancellationCallback?.Invoke();
                }

                if (!canceled) CancellationTokenSourcePool.ReturnToPool(cts);
            }
        }

        public static Cts CallPeriodically<T>(this T obj, double duration, float timeStep, Action<T> action)
        {
            return CallPeriodically(obj, duration, timeStep, action, false, PlayerLoops.Update, null);
        }

        public static Cts CallPeriodically<T>(this T obj, double duration, float timeStep, Action<T> action, GameObject cullingObject)
        {
            return CallPeriodically(obj, duration, timeStep, action, false, PlayerLoops.Update, cullingObject);
        }

        public static Cts CallPeriodically<T>(this T obj, double duration, float timeStep, Action<T> action, PlayerLoops playerLoop)
        {
            return CallPeriodically(obj, duration, timeStep, action, false, playerLoop, null);
        }

        public static Cts CallPeriodically<T>(this T obj, double duration, float timeStep, Action<T> action, bool unscaledTime)
        {
            return CallPeriodically(obj, duration, timeStep, action, unscaledTime, PlayerLoops.Update, null);
        }

        public static Cts CallPeriodically<T>(this T obj, double duration, float timeStep, Action<T> action, bool unscaledTime, PlayerLoops playerLoop)
        {
            return CallPeriodically(obj, duration, timeStep, action, unscaledTime, playerLoop, null);
        }

        public static Cts CallPeriodically<T>(this T obj, double duration, float timeStep, Action<T> action, bool unscaledTime, GameObject cullingObject)
        {
            return CallPeriodically(obj, duration, timeStep, action, unscaledTime, PlayerLoops.Update, cullingObject);
        }

        public static Cts CallPeriodically<T>(this T obj, double duration, float timeStep, Action<T> action, PlayerLoops playerLoop, GameObject cullingObject)
        {
            return CallPeriodically(obj, duration, timeStep, action, false, playerLoop, cullingObject);
        }

        public static Cts CallPeriodically<T>(this T obj, double duration, float timeStep, Action<T> action, bool unscaledTime, PlayerLoops playerLoop, GameObject cullingObject)
        {
            Component mono = obj as Component;
            if(mono != null && cullingObject == null) cullingObject = mono.gameObject;

            var cts = CancellationTokenSourcePool.GetCtsFromPool(cullingObject);
            Task(cullingObject == null ? cts.Token : cts.LinkedTokenSource.Token, obj).Forget();

            return cts;

            async UniTaskVoid Task(CancellationToken token, T component)
            {
                var canceled = false;

                while (duration > 0f && !canceled)
                {
                    action?.Invoke(component);
                    canceled = await UniTask.Delay(TimeSpan.FromSeconds(timeStep), unscaledTime, (PlayerLoopTiming)playerLoop, token).TryAwait();
                    duration -= timeStep;
                }

                if (!canceled) cts?.CompletedCallback?.Invoke();
                else cts?.CancellationCallback?.Invoke();

                if (!canceled) CancellationTokenSourcePool.ReturnToPool(cts);
            }
        }

        public static Cts CallAfterTrue<T>(this T obj, Func<T, bool> condition, Action<T> action)
        {
            return CallAfterTrue(obj, condition, action, PlayerLoops.Update, null);
        }

        public static Cts CallAfterTrue<T>(this T obj, Func<T, bool> condition, Action<T> action, GameObject cullingObject)
        {
            return CallAfterTrue(obj, condition, action, PlayerLoops.Update, cullingObject);
        }

        public static Cts CallAfterTrue<T>(this T obj, Func<T, bool> condition, Action<T> action, PlayerLoops playerLoop, GameObject cullingObject)
        {
            Component mono = obj as Component;
            if (mono != null && cullingObject == null) cullingObject = mono.gameObject;

            var cts = CancellationTokenSourcePool.GetCtsFromPool(cullingObject);
            Task(cullingObject == null ? cts.Token : cts.LinkedTokenSource.Token, obj).Forget();

            return cts;

            async UniTaskVoid Task(CancellationToken token, T component)
            {
                var canceled = await UniTask.WaitUntil(component, condition, (PlayerLoopTiming)playerLoop, token).TryAwait();
                if (!canceled)
                {
                    action?.Invoke(component);
                }

                if (!canceled) cts?.CompletedCallback?.Invoke();
                else cts?.CancellationCallback?.Invoke();

                if (!canceled) CancellationTokenSourcePool.ReturnToPool(cts);
            }
        }

        public static Cts CallWhileTrue<T>(this T obj, Func<T, bool> condition, Action<T> action)
        {
            return CallWhileTrue(obj, condition, action, PlayerLoops.Update, null);
        }

        public static Cts CallWhileTrue<T>(this T obj, Func<T, bool> condition, Action<T> action, GameObject cullingObject)
        {
            return CallWhileTrue(obj, condition, action, PlayerLoops.Update, cullingObject);
        }

        public static Cts CallWhileTrue<T>(this T obj, Func<T, bool> condition, Action<T> action, PlayerLoops playerLoop, GameObject cullingObject)
        {
            Component mono = obj as Component;
            if (mono != null && cullingObject == null) cullingObject = mono.gameObject;

            var cts = CancellationTokenSourcePool.GetCtsFromPool(cullingObject);
            Task(cullingObject == null ? cts.Token : cts.LinkedTokenSource.Token, obj).Forget();

            return cts;

            async UniTaskVoid Task(CancellationToken token, T component)
            {
                var canceled = false;

                while (condition(component) && !canceled)
                {
                    action?.Invoke(component);
                    canceled = await UniTask.Yield((PlayerLoopTiming)playerLoop, token).TryAwait();
                }

                if (!canceled) cts?.CompletedCallback?.Invoke();
                else cts?.CancellationCallback?.Invoke();

                if (!canceled) CancellationTokenSourcePool.ReturnToPool(cts);
            }
        }
    }
}