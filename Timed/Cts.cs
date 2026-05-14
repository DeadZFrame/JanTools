using System;
using System.Threading;

namespace Jan.Tasks
{
    /// <summary>
    /// A specialized CancellationTokenSource that provides additional callback functionality
    /// for both cancellation and completion events. This class is used by the Timed utility
    /// to manage task cancellation while providing hooks for custom logic when operations
    /// are either canceled or completed normally.
    /// </summary>
    public class Cts : CancellationTokenSource
    {
        internal CancellationTokenSource LinkedTokenSource { get; set; }

        /// <summary>
        /// Action that gets invoked when cancellation occurs, either through explicit cancellation or disposal.
        /// </summary>
        internal Action CancellationCallback { get; private set; }
        
        /// <summary>
        /// Action that gets invoked when an operation completes normally without cancellation.
        /// </summary>
        internal Action CompletedCallback { get; private set; }

        internal bool IsDisposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cts"/> class with null callbacks.
        /// </summary>
        public Cts()
        {
            LinkedTokenSource = null;
            CancellationCallback = null;
            CompletedCallback = null;
        }

        /// <summary>
        /// Safely cancels the token source only if cancellation has not already been requested.
        /// This method prevents unnecessary cancellation attempts and potential exceptions when 
        /// cancellation has already occurred.
        /// </summary>
        public void SafeCancel()
        {
            if(!IsCancellationRequested)
            {
                if (LinkedTokenSource == null) Cancel();
                else LinkedTokenSource.Cancel();
            } 
        }

        /// <summary>
        /// Cancels any ongoing operations and disposes this instance to release resources.
        /// If cancellation has not been requested previously, it will be canceled before disposal.
        /// </summary>
        internal void DisposeTask()
        {
            try
            {
                if (!IsCancellationRequested)
                {
                    if (LinkedTokenSource == null) Cancel();
                    else LinkedTokenSource.Cancel();
                }
            }
            finally
            {
                if (LinkedTokenSource == null) Dispose();
                else LinkedTokenSource.Dispose();
                
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Registers a callback action to be executed when cancellation occurs.
        /// </summary>
        /// <param name="action">The callback action to execute when cancellation occurs.</param>
        /// <returns>This instance, allowing for method chaining.</returns>
        public Cts OnCancelled(Action action)
        {
            CancellationCallback += action;
            return this;
        }

        /// <summary>
        /// Registers a callback action to be executed when the operation completes normally.
        /// </summary>
        /// <param name="action">The callback action to execute upon completion.</param>
        /// <returns>This instance, allowing for method chaining.</returns>
        public Cts OnCompleted(Action action)
        {
            CompletedCallback += action;
            return this;
        }

        /// <summary>
        /// Resets this instance for reuse in the object pool.
        /// </summary>
        /// <remarks>
        /// CancellationTokenSource cannot be truly reset after being used,
        /// so this method just ensures it's in a cancelled state and clears callbacks.
        /// The Timed.ReturnToPool method will handle proper object reuse.
        /// </remarks>
        public void Reset()
        {
            // Clear callbacks
            CancellationCallback = null;
            CompletedCallback = null;
            LinkedTokenSource = null;
            IsDisposed = false;
        }
    } 
}

