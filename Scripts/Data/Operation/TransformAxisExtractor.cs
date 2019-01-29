namespace Zinnia.Data.Operation
{
    using UnityEngine;

    /// <summary>
    /// Extracts the chosen axis.
    /// </summary>
    public class TransformAxisExtractor : TransformVector3PropertyExtractor
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

        [Tooltip("The direction to extract from the Transform."), SerializeField]
        private AxisDirection _direction;
        /// <summary>
        /// The direction to extract from the <see cref="Transform"/>.
        /// </summary>
        public AxisDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

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