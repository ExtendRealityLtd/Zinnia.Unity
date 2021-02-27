namespace Zinnia.Data.Type.Transformation.Conversion
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using UnityEngine.Events;

    /// <summary>
    /// Transforms a float value to the equivalent string value.
    /// </summary>
    /// <example>
    /// 1f = "1"
    /// 2.2f = "2.2"
    /// </example>
    public class FloatToString : Transformer<float, string, FloatToString.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="string"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<string> { }

        /// <summary>
        /// The format for the conversion.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public string Format { get; set; }

        /// <summary>
        /// Transforms the given input <see cref="float"/> to the <see cref="string"/> equivalent value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override string Process(float input)
        {
            return input.ToString(Format != null ? Format.Trim() : null);
        }
    }
}