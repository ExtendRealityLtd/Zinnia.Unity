namespace VRTK.Core.Haptics
{
    using UnityEngine;

    /// <summary>
    /// Creates a single pulse on a haptic device for a given intensity.
    /// </summary>
    public abstract class HapticPulse : HapticProcess
    {
        /// <summary>
        /// The intensity of the haptic rumble.
        /// </summary>
        [Tooltip("The intensity of the haptic rumble."), Range(0f, 1f), SerializeField]
        protected float intensity = 1f;

        /// <summary>
        /// The intensity of the haptic rumble.
        /// </summary>
        public float Intensity
        {
            get { return intensity; }
            set { intensity = Mathf.Clamp01(value); }
        }
    }
}