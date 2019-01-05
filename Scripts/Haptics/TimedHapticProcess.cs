namespace Zinnia.Haptics
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Processes a given <see cref="HapticProcess"/> repeatidly for a given duration and with a pause interval between each process.
    /// </summary>
    public class TimedHapticProcess : HapticProcess
    {
        /// <summary>
        /// The process to utilize.
        /// </summary>
        public HapticProcess hapticProcess;
        /// <summary>
        /// The amount of time to keep repeating the process for.
        /// </summary>
        [SerializeField]
        protected float duration = 1f;
        /// <summary>
        /// The amount of time to pause after each process iteration.
        /// </summary>
        [SerializeField]
        protected float interval = 0.1f;

        /// <summary>
        /// The amount of time to keep repeating the process for.
        /// </summary>
        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        /// <summary>
        /// The amount of time to pause after each process iteration.
        /// </summary>
        public float Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        /// <summary>
        /// A reference to the started routine.
        /// </summary>
        protected Coroutine hapticRoutine;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return (base.IsActive() && hapticProcess != null && hapticProcess.IsActive());
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
            hapticProcess.Cancel();
        }

        /// <summary>
        /// Enumerates for the specified duration calling the given <see cref="hapticProcess"/> with a specified interval delay between each call.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected virtual IEnumerator HapticProcessRoutine()
        {
            if (Interval <= 0)
            {
                yield break;
            }

            float currentDuration = Duration;
            WaitForSeconds delay = new WaitForSeconds(Interval);

            while (currentDuration > 0)
            {
                hapticProcess.Begin();
                yield return delay;
                currentDuration -= Interval;
            }
        }
    }
}