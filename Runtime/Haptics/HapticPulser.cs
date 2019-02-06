namespace Zinnia.Haptics
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.PropertySetterMethod;
    using Malimbe.PropertyValidationMethod;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

    /// <summary>
    /// Creates a single pulse on a haptic device for a given intensity.
    /// </summary>
    public abstract class HapticPulser : HapticProcess
    {
        /// <summary>
        /// The intensity of the haptic rumble.
        /// </summary>
        [Serialized, Validated]
        [field: Range(0f, 1f), DocumentedByXml]
        public float Intensity { get; set; } = 1f;

        /// <summary>
        /// Handles changes to <see cref="Intensity"/>.
        /// </summary>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        [CalledBySetter(nameof(Intensity))]
        protected virtual void OnIntensityChange(float previousValue, ref float newValue)
        {
            newValue = Mathf.Clamp01(newValue);
        }
    }
}