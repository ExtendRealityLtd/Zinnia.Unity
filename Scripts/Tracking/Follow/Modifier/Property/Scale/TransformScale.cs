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
        /// Modifies the target scale to match the given source scale.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            if (useLocalScale)
            {
                target.transform.localScale = (offset != null ? (source.transform.localScale - (offset.transform.localScale - target.transform.localScale)) : source.transform.localScale);
            }
            else
            {
                target.transform.SetGlobalScale((offset != null ? (source.transform.lossyScale - (offset.transform.lossyScale - target.transform.lossyScale)) : source.transform.lossyScale));
            }
        }
    }
}