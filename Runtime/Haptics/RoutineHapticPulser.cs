namespace Zinnia.Haptics
{
    using UnityEngine;
    using System.Collections;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Creates a haptic pattern based on a custom routine and utilizes a <see cref="Haptics.HapticPulser"/> to create the effect.
    /// </summary>
    public abstract class RoutineHapticPulser : HapticProcess
    {
        /// <summary>
        /// The pulse process to utilize.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public HapticPulser HapticPulser { get; set; }
        /// <summary>
        /// Multiplies the current audio peak to affect the wave intensity.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float IntensityMultiplier { get; set; } = 1f;

        /// <summary>
        /// A reference to the started routine.
        /// </summary>
        protected Coroutine hapticRoutine;
        /// <summary>
        /// The original intensity of <see cref="HapticPulser"/> to reset back to after the process is complete.
        /// </summary>
        protected float cachedIntensity;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && HapticPulser != null && HapticPulser.IsActive();
        }

        /// <inheritdoc />
        protected override void DoBegin()
        {
            cachedIntensity = HapticPulser.Intensity;
            hapticRoutine = StartCoroutine(HapticProcessRoutine());
        }

        /// <inheritdoc />
        protected override void DoCancel()
        {
            if (hapticRoutine == null)
            {
                return;
            }

            StopCoroutine(hapticRoutine);
            hapticRoutine = null;
            HapticPulser.Cancel();
            ResetIntensity();
        }

        /// <summary>
        /// Resets the <see cref="Haptics.HapticPulser.Intensity"/> back to its original value.
        /// </summary>
        protected virtual void ResetIntensity()
        {
            HapticPulser.Intensity = cachedIntensity;
        }

        /// <summary>
        /// A custom routine to generate a haptic pattern.
        /// </summary>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected abstract IEnumerator HapticProcessRoutine();
    }
}