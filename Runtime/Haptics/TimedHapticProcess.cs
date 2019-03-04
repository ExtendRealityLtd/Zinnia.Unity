﻿namespace Zinnia.Haptics
{
    using UnityEngine;
    using System.Collections;
    using Malimbe.PropertySerializationAttribute;
    /*using Malimbe.PropertyValidationMethod;*/
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Processes a given <see cref="HapticProcess"/> repeatedly for a given duration and with a pause interval between each process.
    /// </summary>
    public class TimedHapticProcess : HapticProcess
    {
        /// <summary>
        /// The process to utilize.
        /// </summary>
        [DocumentedByXml]
        public HapticProcess hapticProcess;

        /// <summary>
        /// The amount of time to keep repeating the process for.
        /// </summary>
        [Serialized, /*Validated*/]
        [field: DocumentedByXml]
        public float Duration { get; set; } = 1f;

        /// <summary>
        /// The amount of time to pause after each process iteration.
        /// </summary>
        [Serialized, /*Validated*/]
        [field: DocumentedByXml]
        public float Interval { get; set; } = 0.1f;

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