namespace Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using UnityEngine;

    /// <summary>
    /// Updates the transform rotation of the target to match the source.
    /// </summary>
    public class TransformRotation : SmoothedRestrictableTransformPropertyModifier
    {
        /// <summary>
        /// Modifies the target rotation to match the given source rotation.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            SaveOriginalPropertyValue(target.transform.eulerAngles);

            Quaternion targetRotation = offset == null ? source.transform.rotation : source.transform.rotation * Quaternion.Inverse(offset.transform.localRotation);

            target.transform.rotation = Smooth(target.transform.rotation, targetRotation);

            if (HasAxisRestrictions)
            {
                target.transform.rotation = Quaternion.Euler(RestrictPropertyValue(target.transform.eulerAngles));
            }
        }
    }
}