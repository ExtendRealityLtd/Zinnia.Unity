namespace VRTK.Core.Haptics
{
    using UnityEngine;

    /// <summary>
    /// Creates a single pulse on a haptic device for a given intensity.
    /// </summary>
    public abstract class HapticPulser : HapticProcess
    {
        [Tooltip("The intensity of the haptic rumble."), Range(0f, 1f), SerializeField]
        private float _intensity = 1f;
        /// <summary>
        /// The intensity of the haptic rumble.
        /// </summary>
        public float Intensity
        {
            get { return _intensity; }
            set { _intensity = Mathf.Clamp01(value); }
        }
    }
}