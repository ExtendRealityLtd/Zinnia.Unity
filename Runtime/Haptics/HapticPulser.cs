namespace Zinnia.Haptics
{
    using Malimbe.MemberChangeMethod;
    using UnityEngine;

    /// <summary>
    /// Creates a single pulse on a haptic device for a given intensity.
    /// </summary>
    public abstract class HapticPulser : HapticProcess
    {
        /// <summary>
        /// The intensity of the haptic rumble.
        /// </summary>
        [Tooltip("The intensity of the haptic rumble.")]
        [SerializeField]
        private float _intensity = 1f;
        public float Intensity
        {
            get
            {
                return _intensity;
            }
            set
            {
                _intensity = value;
            }
        }

        /// <summary>
        /// Called after <see cref="Intensity"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Intensity))]
        protected virtual void OnAfterIntensityChange()
        {
            Intensity = Mathf.Clamp01(Intensity);
        }
    }
}