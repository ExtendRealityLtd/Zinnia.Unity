namespace Zinnia.Tracking.Follow.Modifier.Property.Scale
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Updates the transform scale of the target to match the source.
    /// </summary>
    public class TransformScale : SmoothedRestrictableTransformPropertyModifier
    {
        /// <summary>
        /// Modifies the target scale to match the given source scale.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            SaveOriginalPropertyValue(target.transform.lossyScale);

            Vector3 targetScale = offset == null ? source.transform.lossyScale : source.transform.lossyScale.Divide(offset.transform.localScale);

            target.transform.SetGlobalScale(Smooth(target.transform.lossyScale, targetScale));

            if (HasAxisRestrictions)
            {
                target.transform.SetGlobalScale(RestrictPropertyValue(target.transform.lossyScale));
            }
        }
    }
}