namespace Zinnia.Data.Operation
{
    using Malimbe.PropertySerializationAttribute;
    /*using Malimbe.PropertyValidationMethod;*/
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
        [Serialized, /*Validated*/]
        [field: DocumentedByXml]
        public AxisDirection Direction { get; set; }

        /// <inheritdoc />
        protected override Vector3 ExtractValue()
        {
            switch (Direction)
            {
                case AxisDirection.Right:
                    return useLocal ? Vector3.right : source.transform.right;
                case AxisDirection.Up:
                    return useLocal ? Vector3.up : source.transform.up;
                case AxisDirection.Forward:
                    return useLocal ? Vector3.forward : source.transform.forward;
            }

            return Vector3.zero;
        }
    }
}