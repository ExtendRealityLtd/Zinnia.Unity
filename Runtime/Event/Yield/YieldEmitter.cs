namespace Zinnia.Event.Yield
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Emits an event when a coroutine has complete after the yield.
    /// </summary>
    public abstract class YieldEmitter : MonoBehaviour
    {
        /// <summary>
        /// Emitted when the coroutine has yielded successfully.
        /// </summary>
        public UnityEvent Yielded = new UnityEvent();
        /// <summary>
        /// Emitted when the coroutine is cancelled before the yield completes.
        /// </summary>
        public UnityEvent Cancelled = new UnityEvent();

        /// <summary>
        /// Whether the routine is currently running.
        /// </summary>
        public virtual bool IsRunning => routine != null;

        /// <summary>
        /// The routine to process.
        /// </summary>
        protected Coroutine routine;

        /// <summary>
        /// Cancels any existing yield check before beginning a new one.
        /// </summary>
        public virtual void CancelThenBegin()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Cancel();
            Begin();
        }

        /// <summary>
        /// Starts a new yield check if one is not already running.
        /// </summary>
        public virtual void Begin()
        {
            if (!this.IsValidState())
            {
                return;
            }

            if (routine == null)
            {
                routine = StartCoroutine(Routine());
            }
        }

        /// <summary>
        /// Cancels any existing yield check.
        /// </summary>
        public virtual void Cancel()
        {
            if (routine != null)
            {
                StopCoroutine(routine);
                Cancelled?.Invoke();
                routine = null;
            }
        }

        /// <summary>
        /// The instruction to yield on.
        /// </summary>
        /// <returns>The enumerator to yield on.</returns>
        protected abstract IEnumerator YieldOn();

        protected virtual void OnDisable()
        {
            Cancel();
        }

        /// <summary>
        /// The routine to perform the yield check with.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected virtual IEnumerator Routine()
        {
            yield return YieldOn();
            if (routine != null)
            {
                Yielded?.Invoke();
            }
            routine = null;
        }
    }
}