namespace Zinnia.Rule
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Determines whether a given <see cref="Vector3"/> is equal to the <see cref="Target"/> within a <see cref="Tolerance"/>.
    /// </summary>
    public class Vector3EqualsRule : Vector3Rule
    {
        /// <summary>
        /// The <see cref="Vector3"/> to check equality against.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 Target { get; set; }
        /// <summary>
        /// The tolerance between the two <see cref="Vector3"/> values that can be considered equal.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float Tolerance { get; set; } = float.Epsilon;

        /// <inheritdoc />
        protected override bool Accepts(Vector3 targetVector3)
        {
            return targetVector3.ApproxEquals(Target, Tolerance);
        }
    }
}