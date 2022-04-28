namespace Zinnia.Haptics
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Creates a haptic pattern based on a custom routine and utilizes a <see cref="Haptics.HapticPulser"/> to create the effect.
    /// </summary>
    public abstract class RoutineHapticPulser : HapticProcess
    {
        [Tooltip("The pulse process to utilize.")]
        [SerializeField]
        private HapticPulser hapticPulser;
        /// <summary>
        /// The pulse process to utilize.
        /// </summary>
        public HapticPulser HapticPulser
        {
            get
            {
                return hapticPulser;
            }
            set
            {
                hapticPulser = value;
            }
        }
        [Tooltip("Multiplies the current audio peak to affect the wave intensity.")]
        [SerializeField]
        private float intensityMultiplier = 1f;
        /// <summary>
        /// Multiplies the current audio peak to affect the wave intensity.
        /// </summary>
        public float IntensityMultiplier
        {
            get
            {
                return intensityMultiplier;
            }
            set
            {
                intensityMultiplier = value;
            }
        }

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