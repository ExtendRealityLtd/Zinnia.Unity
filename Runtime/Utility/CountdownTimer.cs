namespace Zinnia.Utility
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Counts down from a given start time until zero and emits appropriate events throughout the process.
    /// </summary>
    public class CountdownTimer : MonoBehaviour
    {
        /// <summary>
        /// The source of Time to use.
        /// </summary>
        public enum TimeSourceType
        {
            /// <summary>
            /// Taken from <see cref="Time.time"/>.
            /// </summary>
            ScaledTime,
            /// <summary>
            /// Taken from <see cref="Time.unscaledTime"/>.
            /// </summary>
            UnscaledTime,
            /// <summary>
            /// Taken from <see cref="Time.fixedTime"/>.
            /// </summary>
            FixedScaledTime,
            /// <summary>
            /// Taken from <see cref="Time.fixedUnscaledTime"/>.
            /// </summary>
            FixedUnscaledTime,
            /// <summary>
            /// Taken from <see cref="Time.realtimeSinceStartup"/>.
            /// </summary>
            RealTime
        }

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

        [Tooltip("The source for the time to be used in the countdown.")]
        [SerializeField]
        private TimeSourceType timeSource;
        /// <summary>
        /// The source for the time to be used in the countdown.
        /// </summary>
        public TimeSourceType TimeSource
        {
            get
            {
                return timeSource;
            }
            set
            {
                timeSourceChanged = !timeSource.Equals(value);
                timeSource = value;
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
        /// Emitted when the countdown is paused.
        /// </summary>
        public UnityEvent Paused = new UnityEvent();
        /// <summary>
        /// Emitted when the countdown is resumed.
        /// </summary>
        public UnityEvent Resumed = new UnityEvent();
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
        /// Determines if the countdown is currently paused.
        /// </summary>
        public bool IsPaused { get; protected set; }

        /// <summary>
        /// Elapsed time of the timer.
        /// </summary>
        public float ElapsedTime
        {
            get
            {
                if (IsRunning && !IsPaused)
                {
                    currentTime = actualTime;
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
                if (IsRunning && !IsPaused)
                {
                    currentTime = actualTime;
                }
                return StartTime + (beginTime - currentTime);
            }
        }

        /// <summary>
        /// <see cref="actualTime"/> when <see cref="Begin"/> is called.
        /// </summary>
        protected float beginTime;
        /// <summary>
        /// <see cref="actualTime"/> of the current frame.
        /// </summary>
        protected float currentTime;

        /// <summary>
        /// The <see cref="RemainingTime"/> at the point of calling <see cref="Pause"/>.
        /// </summary>
        protected float remainingAtPauseTime;

        /// <summary>
        /// Whether the <see cref="TimeSource"/> has changed.
        /// </summary>
        protected bool timeSourceChanged = true;

        /// <summary>
        /// The stored function for retrieving the time.
        /// </summary>
        protected Func<float> selectedTimeFunction = () => Time.time;

        /// <summary>
        /// The actual time value based on the selected <see cref="TimeSource"/>.
        /// </summary>
        protected float actualTime
        {
            get
            {
                if (timeSourceChanged)
                {
                    switch(TimeSource)
                    {
                        case TimeSourceType.ScaledTime:
                            selectedTimeFunction = () => Time.time;
                            break;
                        case TimeSourceType.UnscaledTime:
                            selectedTimeFunction = () => Time.unscaledTime;
                            break;
                        case TimeSourceType.FixedScaledTime:
                            selectedTimeFunction = () => Time.fixedTime;
                            break;
                        case TimeSourceType.FixedUnscaledTime:
                            selectedTimeFunction = () => Time.fixedUnscaledTime;
                            break;
                        case TimeSourceType.RealTime:
                            selectedTimeFunction = () => Time.realtimeSinceStartup;
                            break;
                    }
                    timeSourceChanged = false;
                }

                return selectedTimeFunction();
            }
        }

        /// <summary>
        /// A container to hold the timer coroutine.
        /// </summary>
        protected Coroutine timerRoutine;

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
            StartTimer(StartTime);
            Started?.Invoke();
        }

        /// <summary>
        /// Cancels the timer counting down.
        /// </summary>
        public virtual void Cancel()
        {
            CancelRoutine();
            //CancelInvoke(nameof(Complete));
            if (IsRunning)
            {
                currentTime = actualTime;
                Cancelled?.Invoke();
                IsRunning = false;
                IsPaused = false;
                remainingAtPauseTime = 0f;
            }
        }

        /// <summary>
        /// Pauses the current countdown timer.
        /// </summary>
        public virtual void Pause()
        {
            if (!IsRunning || IsPaused)
            {
                return;
            }

            remainingAtPauseTime = RemainingTime;
            IsPaused = true;
            CancelRoutine();
            Paused?.Invoke();
        }

        /// <summary>
        /// resumes the current countdown timer from pause.
        /// </summary>
        public virtual void Resume()
        {
            if (!IsRunning || !IsPaused)
            {
                return;
            }

            StartTimer(remainingAtPauseTime);
            IsPaused = false;
            Resumed?.Invoke();
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
        /// Starts the countdown timer.
        /// </summary>
        /// <param name="invokeTime">The time to delay until the invoke is executed.</param>
        protected virtual void StartTimer(float invokeTime)
        {
            SetInternalStates();
            CancelRoutine();
            timerRoutine = StartCoroutine(StartRoutine(invokeTime));
        }

        /// <summary>
        /// Starts the timer routine.
        /// </summary>
        /// <param name="invokeTime">The time to wait until completion.</param>
        /// <returns>The enumerator for the coroutine.</returns>
        protected virtual IEnumerator StartRoutine(float invokeTime)
        {
            float targetTime = actualTime + invokeTime;
            while (actualTime < targetTime)
            {
                yield return null;
            }
            Complete();
        }

        /// <summary>
        /// Cancels the timer routine.
        /// </summary>
        protected virtual void CancelRoutine()
        {
            if (timerRoutine != null)
            {
                StopCoroutine(timerRoutine);
            }

            timerRoutine = null;
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
        /// Stores current <see cref="actualTime"/> for calculations.
        /// </summary>
        protected virtual void SetInternalStates()
        {
            beginTime = actualTime;
            currentTime = actualTime;
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