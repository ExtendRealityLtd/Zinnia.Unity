namespace Zinnia.Utility
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Counts down from a given start time until zero and emits appropriate events throughout the process.
    /// </summary>
    public class CountdownTimer : MonoBehaviour
    {
        /// <summary>
        /// The time to start the countdown at.
        /// </summary>
        [DocumentedByXml]
        public float startTime = 1f;

        /// <summary>
        /// Emitted when the countdown starts.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Started = new UnityEvent();
        /// <summary>
        /// Emitted when the countdown is cancelled.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Cancelled = new UnityEvent();
        /// <summary>
        /// Emitted when the countdown completes.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Completed = new UnityEvent();
        /// <summary>
        /// Emitted when the status of the countdown is checked and is still running.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent StillRunning = new UnityEvent();
        /// <summary>
        /// Emitted when the status of the countdown is checked and is not running.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent NotRunning = new UnityEvent();

        /// <summary>
        /// Determines if the countdown is still running.
        /// </summary>
        public bool IsRunning
        {
            get;
            protected set;
        }

        /// <summary>
        /// Starts the timer counting down.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Begin()
        {
            IsRunning = true;
            Invoke(nameof(Complete), startTime);
            Started?.Invoke();
        }

        /// <summary>
        /// Cancels the timer counting down.
        /// </summary>
        public virtual void Cancel()
        {
            IsRunning = false;
            CancelInvoke(nameof(Complete));
            Cancelled?.Invoke();
        }

        /// <summary>
        /// Emits the current running status of the timer.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void EmitStatus()
        {
            if (IsRunning)
            {
                StillRunning?.Invoke();
            }
            else
            {
                NotRunning?.Invoke();
            }
        }

        protected virtual void OnDisable()
        {
            Cancel();
        }

        /// <summary>
        /// Executed when the countdown is complete.
        /// </summary>
        protected virtual void Complete()
        {
            IsRunning = false;
            Completed?.Invoke();
        }
    }
}