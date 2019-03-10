﻿namespace Zinnia.Haptics
{
    using UnityEngine;
    using System.Collections;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Processes a given <see cref="Haptics.HapticProcess"/> repeatedly for a given duration and with a pause interval between each process.
    /// </summary>
    public class TimedHapticProcess : HapticProcess
    {
        /// <summary>
        /// The process to utilize.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public HapticProcess HapticProcess { get; set; }

        /// <summary>
        /// The amount of time to keep repeating the process for.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float Duration { get; set; } = 1f;

        /// <summary>
        /// The amount of time to pause after each process iteration.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float Interval { get; set; } = 0.1f;

        /// <summary>
        /// A reference to the started routine.
        /// </summary>
        protected Coroutine hapticRoutine;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && HapticProcess != null && HapticProcess.IsActive();
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
            WaitForSeconds delay = new WaitForSeconds(Interval);

            while (currentDuration > 0)
            {
                HapticProcess.Begin();
                yield return delay;
                currentDuration -= Interval;
            }
        }
    }
}