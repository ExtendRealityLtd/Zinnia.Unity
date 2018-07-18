namespace VRTK.Core.Tracking.Follow.Modifier.Property.Rotation
{
    using UnityEngine;

    /// <summary>
    /// Updates the transform rotation of the target to match the source.
    /// </summary>
    public class TransformRotation : PropertyModifier
    {
        /// <summary>
        /// Determines whether to use local rotation or global rotation.
        /// </summary>
        [Tooltip("Determines whether to use rotation rotation or global rotation.")]
        public bool useLocalRotation;

        /// <summary>
        /// Modifies the target <see cref="Transform.rotation"/> to match the given source <see cref="Transform.rotation"/>.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(Transform source, Transform target, Transform offset = null)
        {
            if (useLocalRotation)
            {
                target.localRotation = (offset != null ? source.localRotation * Quaternion.Inverse(offset.localRotation) : source.localRotation);
            }
            else
            {
                target.rotation = (offset != null ? source.rotation * Quaternion.Inverse(offset.localRotation) : source.rotation);
            }
        }
    }
}