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
        /// Modifies the target <see cref="Transform.position"/> to match the given source <see cref="Transform.position"/>.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(Transform source, Transform target, Transform offset = null)
        {
            if (useLocalPosition)
            {
                target.localPosition = (offset != null ? (source.localPosition - (offset.localPosition - target.localPosition)) : source.localPosition);
            }
            else
            {
                target.position = (offset != null ? (source.position - (offset.position - target.position)) : source.position);
            }
        }
    }
}