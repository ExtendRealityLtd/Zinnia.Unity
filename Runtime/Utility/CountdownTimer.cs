namespace Zinnia.Utility
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Counts down from a given start time until zero and emits appropriate events throughout the process.
    /// </summary>
    public class CountdownTimer : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the specified <see cref="float"/>.
        /// </summary>
        [Serializable]
        public class FloatUnityEvent : UnityEvent<float> { }

        #region Timer Settings
        [Header("Timer Settings")]
        [Tooltip("The time to start the countdown at.")]
        [SerializeField]
        private float startTime = 1f;
        /// <summary>
        /// The time to start the countdown at.
        /// </summary>
        public float StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterStartTimeChange();
                }
            }
        }
        [Tooltip("Whether to start the countdown timer when the component becomes enabled.")]
        [SerializeField]
        private bool beginOnEnable;
        /// <summary>
        /// Whether to start the countdown timer when the component becomes enabled.
        /// </summary>
        public bool BeginOnEnable
        {
            get
            {
                return beginOnEnable;
            }
            set
            {
                beginOnEnable = value;
            }
        }
        #endregion

        #region Timer Events
        /// <summary>
        /// Emitted when the countdown starts.
        /// </summary>
        [Header("Timer Events")]
        public UnityEvent Started = new UnityEvent();
        /// <summary>
        /// Emitted when the countdown is canceled.
        /// </summary>
        public UnityEvent Cancelled = new UnityEvent();
        /// <summary>
        /// Emitted when the countdown completes.
        /// </summary>
        public UnityEvent Completed = new UnityEvent();
        /// <summary>
        /// Emitted when the status of the countdown is checked and is still running.
        /// </summary>
        public UnityEvent StillRunning = new UnityEvent();
        /// <summary>
        /// Emitted when the status of the countdown is checked and is not running.
        /// </summary>
        public UnityEvent NotRunning = new UnityEvent();
        /// <summary>
        /// Emitted when the elapsed time is checked.
        /// </summary>
        public FloatUnityEvent ElapsedTimeEmitted = new FloatUnityEvent();
        /// <summary>
        /// Emitted when the remaining time is checked.
        /// </summary>
        public FloatUnityEvent RemainingTimeEmitted = new FloatUnityEvent();
        #endregion

        /// <summary>
        /// Determines if the countdown is still running.
        /// </summary>
        public bool IsRunning { get; protected set; }

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
        public virtual void Begin()
        {
            if (!this.IsValidState())
            {
                return;
            }

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
        public virtual void EmitStatus()
        {
            if (!this.IsValidState())
            {
                return;
            }

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
        public virtual void EmitElapsedTime()
        {
            if (!this.IsValidState())
            {
                return;
            }

            ElapsedTimeEmitted?.Invoke(ElapsedTime);
        }

        /// <summary>
        /// Emits the remaining time of the timer.
        /// </summary>
        public virtual void EmitRemainingTime()
        {
            if (!this.IsValidState())
            {
                return;
            }

            RemainingTimeEmitted?.Invoke(RemainingTime);
        }

        protected virtual void OnEnable()
        {
            SetInternalStates();
            if (BeginOnEnable)
            {
                Begin();
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
        protected virtual void OnAfterStartTimeChange()
        {
            SetInternalStates();
        }
    }
}