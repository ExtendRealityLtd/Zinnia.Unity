namespace Zinnia.Utility
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberChangeMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Counts down from a given start time until zero and emits appropriate events throughout the process.
    /// </summary>
    public class CountdownTimer : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the specified <see cref="float"/>.
        /// </summary>
        [Serializable]
        public class FloatUnityEvent : UnityEvent<float>
        {
        }

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
        /// Emitted when the elapsed time is checked.
        /// </summary>
        [DocumentedByXml]
        public FloatUnityEvent ElapsedTimeEmitted = new FloatUnityEvent();
        /// <summary>
        /// Emitted when the remaining time is checked.
        /// </summary>
        [DocumentedByXml]
        public FloatUnityEvent RemainingTimeEmitted = new FloatUnityEvent();

        /// <summary>
        /// Determines if the countdown is still running.
        /// </summary>
        public bool IsRunning
        {
            get;
            protected set;
        }

        /// <summary>
        /// Elapsed time of the timer.
        /// </summary>
        public float ElapsedTime
        {
            get
            {
                if (IsRunning)
                {
                    currentTime = Time.time;
                }
                return currentTime - beginTime;
            }
        }

        /// <summary>
        /// Remaining time of the timer.
        /// </summary>
        public float RemainingTime
        {
            get
            {
                if (IsRunning)
                {
                    currentTime = Time.time;
                }
                return StartTime + (beginTime - currentTime);
            }
        }

        /// <summary>
        /// <see cref="Time.time"/> when <see cref="Begin"/> is called.
        /// </summary>
        protected float beginTime;
        /// <summary>
        /// <see cref="Time.time"/> of the current frame.
        /// </summary>
        protected float currentTime;

        /// <summary>
        /// Starts the timer counting down.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Begin()
        {
            IsRunning = true;
            SetInternalStates();
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
                currentTime = Time.time;
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

        /// <summary>
        /// Emits the elapsed time of the timer.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void EmitElapsedTime()
        {
            ElapsedTimeEmitted?.Invoke(ElapsedTime);
        }

        /// <summary>
        /// Emits the remaining time of the timer.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void EmitRemainingTime()
        {
            RemainingTimeEmitted?.Invoke(RemainingTime);
        }

        protected virtual void OnEnable()
        {
            SetInternalStates();
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
            currentTime = StartTime + beginTime;
            IsRunning = false;
            Completed?.Invoke();
        }

        /// <summary>
        /// Stores current <see cref="Time.time"/> for calculations.
        /// </summary>
        protected virtual void SetInternalStates()
        {
            beginTime = Time.time;
            currentTime = Time.time;
        }

        /// <summary>
        /// Called after <see cref="StartTime"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(StartTime))]
        protected virtual void OnAfterStartTimeChange()
        {
            SetInternalStates();
        }
    }
}