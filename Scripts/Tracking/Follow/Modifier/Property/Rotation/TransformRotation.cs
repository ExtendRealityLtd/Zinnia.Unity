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
        /// Modifies the target rotation to match the given source rotation.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            if (useLocalRotation)
            {
                target.transform.localRotation = (offset != null ? source.transform.localRotation * Quaternion.Inverse(offset.transform.localRotation) : source.transform.localRotation);
            }
            else
            {
                target.transform.rotation = (offset != null ? source.transform.rotation * Quaternion.Inverse(offset.transform.localRotation) : source.transform.rotation);
            }
        }
    }
}