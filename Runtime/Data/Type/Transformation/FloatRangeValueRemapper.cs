namespace Zinnia.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Transforms a <see cref="float"/> by remapping from a range to a new range.
    /// </summary>
    /// <example>
    /// 2f -> From(0f, 10f), To(0f, 1f), Mode(Lerp) = 0.2f
    /// 2f -> From(0f, 10f), To(1f, 0f), Mode(Lerp) = 0.8f
    /// 2f -> From(10f, 0f), To(0f, 1f), Mode(Lerp) = 0.8f
    /// 2f -> From(10f, 0f), To(1f, 0f), Mode(Lerp) = 0.2f
    /// 2f -> From(0f, 10f), To(0f, 1f), Mode(SmoothStep) = 0.104f
    /// </example>
    public class FloatRangeValueRemapper : Transformer<float, float, FloatRangeValueRemapper.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the remapped <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float>
        {
        }

        /// <summary>
        /// The range of the value from.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public FloatRange From { get; set; } = new FloatRange(0f, 1f);

        /// <summary>
        /// The range of the value remaps to.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public FloatRange To { get; set; } = new FloatRange(0f, 1f);

        /// <summary>
        /// The mode to use when remapping.
        /// </summary>
        public enum OutputMode
        {
            /// <summary>
            /// Linearly interpolates.
            /// </summary>
            Lerp,
            /// <summary>
            /// Interpolates with smoothing at the limits
            /// </summary>
            SmoothStep
        }

        /// <summary>
        /// The mode to use when remapping.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public OutputMode Mode { get; set; } = OutputMode.Lerp;

        /// <summary>
        /// Transforms the given <see cref="float"/> by remapping to a new range.
        /// </summary>
        /// <param name="input">The value to remap.</param>
        /// <returns>A new <see cref="float"/> remapped.</returns>
        protected override float Process(float input)
        {
            float t = Mathf.InverseLerp(From.minimum, From.maximum, input);
            return Mode == OutputMode.Lerp? 
                Mathf.Lerp(To.minimum, To.maximum, t) : 
                Mathf.SmoothStep(To.minimum, To.maximum, t);
        }
    }
}