namespace VRTK.Core.Tracking.Follow.Modifier.Property.Position
{
    using UnityEngine;

    /// <summary>
    /// Updates the transform position of the target to match the source.
    /// </summary>
    public class TransformPosition : PropertyModifier
    {
        /// <summary>
        /// Determines whether to use local position or global position.
        /// </summary>
        [Tooltip("Determines whether to use local position or global position.")]
        public bool useLocalPosition;

        /// <summary>
        /// Modifies the target position to match the given source position.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            if (useLocalPosition)
            {
                target.transform.localPosition = (offset != null ? (source.transform.localPosition - (offset.transform.localPosition - target.transform.localPosition)) : source.transform.localPosition);
            }
            else
            {
                target.transform.position = (offset != null ? (source.transform.position - (offset.transform.position - target.transform.position)) : source.transform.position);
            }
        }
    }
}