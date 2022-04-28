namespace Zinnia.Haptics
{
    using System;
    using System.Collections;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Processes a given <see cref="Haptics.HapticProcess"/> repeatedly for a given duration and with a pause interval between each process.
    /// </summary>
    public class TimedHapticProcess : HapticProcess
    {
        [Tooltip("The process to utilize.")]
        [SerializeField]
        private HapticProcess hapticProcess;
        /// <summary>
        /// The process to utilize.
        /// </summary>
        public HapticProcess HapticProcess
        {
            get
            {
                return hapticProcess;
            }
            set
            {
                hapticProcess = value;
            }
        }
        [Tooltip("The amount of time to keep repeating the process for.")]
        [SerializeField]
        private float duration = 1f;
        /// <summary>
        /// The amount of time to keep repeating the process for.
        /// </summary>
        public float Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
            }
        }
        [Tooltip("The amount of time to pause after each process iteration.")]
        [SerializeField]
        private float interval = 0.1f;
        /// <summary>
        /// The amount of time to pause after each process iteration.
        /// </summary>
        public float Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterIntervalChange();
                }
            }
        }

        /// <summary>
        /// A reference to the started routine.
        /// </summary>
        protected Coroutine hapticRoutine;
        /// <summary>
        /// Delays the <see cref="hapticRoutine"/> by <see cref="Interval"/> seconds.
        /// </summary>
        protected WaitForSeconds delayYieldInstruction;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && HapticProcess != null && HapticProcess.IsActive();
        }

        protected virtual void OnEnable()
        {
            OnAfterIntervalChange();
        }

        /// <summary>
        /// Starts the haptic routine.
        /// </summary>
        protected override void DoBegin()
        {
            hapticRoutine = StartCoroutine(HapticProcessRoutine());
        }

        /// <summary>
        /// Cancels any current running haptic routine.
        /// </summary>
        protected override void DoCancel()
        {
            if (hapticRoutine == null)
            {
                return;
            }

            StopCoroutine(hapticRoutine);
            hapticRoutine = null;
            HapticProcess.Cancel();
        }

        /// <summary>
        /// Enumerates for the specified duration calling <see cref="HapticProcess"/> with a specified interval delay between each call.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected virtual IEnumerator HapticProcessRoutine()
        {
            if (Interval <= 0)
            {
                yield break;
            }

            float currentDuration = Duration;

            while (currentDuration > 0)
            {
                HapticProcess.Begin();
                yield return delayYieldInstruction;
                currentDuration -= Interval;
            }
        }

        /// <summary>
        /// Called after <see cref="Interval"/> has been changed.
        /// </summary>
        protected virtual void OnAfterIntervalChange()
        {
            delayYieldInstruction = new WaitForSeconds(Interval);
        }

        [Obsolete("Use `OnAfterIntervalChange` instead.")]
        protected virtual void OnAfterCheckDelayChange()
        {
            OnAfterIntervalChange();
        }
    }
}