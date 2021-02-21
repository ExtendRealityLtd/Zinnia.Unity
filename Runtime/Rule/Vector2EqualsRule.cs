namespace Zinnia.Rule
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Determines whether a given <see cref="Vector2"/> is equal to the <see cref="Target"/> within a <see cref="Tolerance"/>.
    /// </summary>
    public class Vector2EqualsRule : Vector2Rule
    {
        /// <summary>
        /// The <see cref="Vector2"/> to check equality against.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector2 Target { get; set; }
        /// <summary>
        /// The tolerance between the two <see cref="Vector2"/> values that can be considered equal.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float Tolerance { get; set; } = float.Epsilon;

        /// <inheritdoc />
        protected override bool Accepts(Vector2 targetVector2)
        {
            return targetVector2.ApproxEquals(Target, Tolerance);
        }
    }
}