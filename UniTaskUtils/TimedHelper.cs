using System;
using Cysharp.Threading.Tasks;

namespace Jan.Tasks
{
    public static class TimedHelper
    {
        /// <summary>
        /// Helper method to execute a UniTask and catch OperationCanceledException without allocation overhead.
        /// Replaces SuppressCancellationThrow() to reduce GC pressure.
        /// </summary>
        /// <param name="task">The task to execute</param>
        /// <returns>True if the operation was canceled, false otherwise</returns>
        internal static async UniTask<bool> TryAwait(this UniTask task)
        {
            try
            {
                await task;
                return false; // Not canceled
            }
            catch (OperationCanceledException)
            {
                return true; // Canceled
            }
        }
    }
}