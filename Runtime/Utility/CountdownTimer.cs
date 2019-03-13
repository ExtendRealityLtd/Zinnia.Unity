namespace Zinnia.Utility
{
    using UnityEngine;
    using UnityEngine.Events;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Counts down from a given start time until zero and emits appropriate events throughout the process.
    /// </summary>
    public class CountdownTimer : MonoBehaviour
    {
        /// <summary>
        /// The time to start the countdown at.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float StartTime { get; set; } = 1f;

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
            Invoke(nameof(Complete), StartTime);
            Started?.Invoke();
        }

        /// <summary>
        /// Cancels the timer counting down.
        /// </summary>
        public virtual void Cancel()
        {
            CancelInvoke(nameof(Complete));
            if (IsRunning)
            {
                Cancelled?.Invoke();
                IsRunning = false;
            }
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