namespace Zinnia.Tracking.Follow.Modifier.Property.Scale
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Updates the transform scale of the target to match the source.
    /// </summary>
    public class TransformScale : RestrictableTransformPropertyModifier
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
            if (offset == null)
            {
                target.transform.SetGlobalScale(source.transform.lossyScale);
            }
            else
            {
                target.transform.SetGlobalScale(source.transform.lossyScale.Divide(offset.transform.localScale));
            }

            if (HasAxisRestrictions)
            {
                target.transform.SetGlobalScale(RestrictPropertyValue(target.transform.lossyScale));
            }
        }
    }
}