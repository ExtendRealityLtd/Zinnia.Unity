namespace Zinnia.Haptics
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Creates a single pulse on a haptic device for a given intensity.
    /// </summary>
    public abstract class HapticPulser : HapticProcess
    {
        [Tooltip("The intensity of the haptic rumble.")]
        [SerializeField]
        private float _intensity = 1f;
        /// <summary>
        /// The intensity of the haptic rumble.
        /// </summary>
        public float Intensity
        {
            get
            {
                return _intensity;
            }
            set
            {
                _intensity = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterIntensityChange();
                }
            }
        }

        /// <summary>
        /// Called after <see cref="Intensity"/> has been changed.
        /// </summary>
        protected virtual void OnAfterIntensityChange()
        {
            _intensity = Mathf.Clamp01(Intensity);
        }
    }
}