namespace VRTK.Core.Tracking.Follow.Modifier.Property.Scale
{
    using UnityEngine;
    using VRTK.Core.Extension;

    /// <summary>
    /// Updates the transform scale of the target to match the source.
    /// </summary>
    public class TransformScale : PropertyModifier
    {
        /// <summary>
        /// Determines whether to use local scale or global scale.
        /// </summary>
        [Tooltip("Determines whether to use local scale or global scale.")]
        public bool useLocalScale;

        /// <summary>
        /// Modifies the target <see cref="Transform.localScale"/> to match the given source <see cref="Transform.localScale"/>.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(Transform source, Transform target, Transform offset = null)
        {
            if (useLocalScale)
            {
                target.localScale = (offset != null ? (source.localScale - (offset.localScale - target.localScale)) : source.localScale);
            }
            else
            {
                target.SetGlobalScale((offset != null ? (source.lossyScale - (offset.lossyScale - target.lossyScale)) : source.lossyScale));
            }
        }
    }
}