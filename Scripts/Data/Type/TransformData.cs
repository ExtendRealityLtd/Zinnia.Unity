namespace VRTK.Core.Data.Type
{
    using UnityEngine;

    /// <summary>
    /// The TrasnformData holds Transform information with the ability to override properties without affecting the scene Transform.
    /// </summary>
    public class TransformData
    {
        /// <summary>
        /// A reference to the original transform.
        /// </summary>
        public Transform transform;
        /// <summary>
        /// Position override of the Transform object.
        /// </summary>
        public Vector3? positionOverride = null;
        /// <summary>
        /// Rotation override of the Transform object.
        /// </summary>
        public Quaternion? rotationOverride = null;
        /// <summary>
        /// Scale override of the Transform object.
        /// </summary>
        public Vector3? localScaleOverride = null;

        /// <summary>
        /// The position of the transform or the positionOverride if it is set.
        /// </summary>
        public Vector3 Position => positionOverride ?? transform.position;

        /// <summary>
        /// The rotation of the transform or the rotationOverride if it is set.
        /// </summary>
        public Quaternion Rotation => rotationOverride ?? transform.rotation;

        /// <summary>
        /// The localScale of the transform or the localScaleOverride if it is set.
        /// </summary>
        public Vector3 LocalScale => localScaleOverride ?? transform.localScale;

        /// <summary>
        /// The state of whether the TransformData is valid.
        /// </summary>
        public bool Valid
        {
            get
            {
                return (transform != null);
            }
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public TransformData()
        {
        }

        /// <summary>
        /// Creates a new TransformData from an existing Transform.
        /// </summary>
        /// <param name="source">The Transform to create the TransformData from.</param>
        public TransformData(Transform source)
        {
            transform = source;
        }
    }
}