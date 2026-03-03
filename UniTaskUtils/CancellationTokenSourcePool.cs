using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Jan.Tasks
{
    public static class CancellationTokenSourcePool
    {
        /// <summary>
        /// A static queue that pools and reuses <see cref="Cts"/> objects to reduce memory allocations
        /// and improve performance when creating or managing cancellation tokens in timed operations.
        /// </summary>
        /// <remarks>
        /// The CancellationTokenPool is utilized in various methods of the <c>Timed</c> class to manage task cancellations efficiently,
        /// especially during repetitive or time-sensitive tasks like periodic calls, delays, or frame-based operations.
        /// When a CancellationTokenSource is no longer in use, it can be returned back to the pool for reuse.
        /// </remarks>
        private static readonly Queue<Cts> CancellationTokenPool = new Queue<Cts>();
        
        /// <summary>
        /// Gets a Cts object from the pool or creates a new one if the pool is empty.
        /// </summary>
        /// <returns>A Cts object ready for use.</returns>
        internal static Cts GetCtsFromPool(GameObject cullingObject)
        {
            Cts cts;
            
            if (CancellationTokenPool.Count > 0)
            {
                cts = CancellationTokenPool.Dequeue();
            }
            else
            {
                cts = new Cts();
            }
            
            if (cullingObject != null)
            {
                var cullingToken = cullingObject.GetCancellationTokenOnDestroy();
                cts.LinkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cullingToken, cts.Token);
            }

            return cts;
        }
        
        internal static Cts GetCtsFromPool(MonoBehaviour cullingObject)
        {
            Cts cts;
            
            if (CancellationTokenPool.Count > 0)
            {
                cts = CancellationTokenPool.Dequeue();
            }
            else
            {
                cts = new Cts();
            }
            
            if (cullingObject != null)
            {
                var cullingToken = cullingObject.GetCancellationTokenOnDestroy();
                cts.LinkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cullingToken, cts.Token);
            }

            return cts;
        }
        
        /// <summary>
        /// Returns a Cts object to the pool for reuse after resetting it.
        /// </summary>
        /// <param name="cts">The Cts object to return to the pool.</param>
        internal static void ReturnToPool(Cts cts)
        {
            if (cts == null) return;

            // if (cts.IsCancellationRequested)
            // {
            //     cts.DisposeTask();
            //     return;
            // }

            // // Reset the token before returning to the pool
            // cts.Reset();
            // CancellationTokenPool.Enqueue(cts);
        }

        internal static void Clear()
        {
            while (CancellationTokenPool.Count > 0)
            {
                var cts = CancellationTokenPool.Dequeue();
                cts.DisposeTask();
            }
        }
    }
}