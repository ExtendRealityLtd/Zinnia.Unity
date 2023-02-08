namespace Zinnia.Tracking.Follow.Modifier.Property.Position
{
    using UnityEngine;

    /// <summary>
    /// Updates the transform position of the target to match the source.
    /// </summary>
    public class TransformPosition : SmoothedRestrictableTransformPropertyModifier
    {
        /// <summary>
        /// Modifies the target position to match the given source position.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            SaveOriginalPropertyValue(target.transform.position);

            Vector3 targetPosition = offset == null ? source.transform.position : source.transform.position - (offset.transform.position - target.transform.position);

            target.transform.position = Smooth(target.transform.position, targetPosition);

            if (HasAxisRestrictions)
            {
                target.transform.position = RestrictPropertyValue(target.transform.position);
            }
        }
    }
}