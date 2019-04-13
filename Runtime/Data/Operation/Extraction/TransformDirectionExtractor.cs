namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Extracts a chosen axis of a <see cref="Transform"/>.
    /// </summary>
    public class TransformDirectionExtractor : TransformVector3PropertyExtractor
    {
        /// <summary>
        /// The direction axes of the transform.
        /// </summary>
        public enum AxisDirection
        {
            /// <summary>
            /// The axis moving right from the transform origin.
            /// </summary>
            Right,
            /// <summary>
            /// The axis moving up from the transform origin.
            /// </summary>
            Up,
            /// <summary>
            /// The axis moving forward from the transform origin.
            /// </summary>
            Forward
        }

        /// <summary>
        /// The direction to extract from the <see cref="Transform"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public AxisDirection Direction { get; set; }

        /// <inheritdoc />
        protected override Vector3 ExtractValue()
        {
            switch (Direction)
            {
                case AxisDirection.Right:
                    return UseLocal ? Vector3.right : Source.transform.right;
                case AxisDirection.Up:
                    return UseLocal ? Vector3.up : Source.transform.up;
                case AxisDirection.Forward:
                    return UseLocal ? Vector3.forward : Source.transform.forward;
            }

            return Vector3.zero;
        }
    }
}