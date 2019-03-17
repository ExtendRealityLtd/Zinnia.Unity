namespace Zinnia.Haptics
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.MemberChangeMethod;

    /// <summary>
    /// Creates a single pulse on a haptic device for a given intensity.
    /// </summary>
    public abstract class HapticPulser : HapticProcess
    {
        /// <summary>
        /// The intensity of the haptic rumble.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml, Range(0f, 1f)]
        public float Intensity { get; set; } = 1f;

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