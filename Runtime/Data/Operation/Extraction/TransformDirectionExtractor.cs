namespace Zinnia.Data.Operation.Extraction
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

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
        protected override Vector3? ExtractValue()
        {
            if (Source == null)
            {
                return null;
            }

            switch (Direction)
            {
                case AxisDirection.Right:
                    return UseLocal ? Source.transform.right : Vector3.right;
                case AxisDirection.Up:
                    return UseLocal ? Source.transform.up : Vector3.up;
                case AxisDirection.Forward:
                    return UseLocal ? Source.transform.forward : Vector3.forward;
            }

            return Vector3.zero;
        }
    }
}