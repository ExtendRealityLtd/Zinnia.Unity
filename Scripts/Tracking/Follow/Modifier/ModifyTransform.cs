namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;

    /// <summary>
    /// Modifies the target <see cref="Transform"/> data to equal the source <see cref="Transform"/> data.
    /// </summary>
    public class ModifyTransform : FollowModifier
    {
        /// <summary>
        /// Sets the target <see cref="Transform.position"/> to the source <see cref="Transform.position"/>.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoUpdatePosition(Transform source, Transform target, Transform offset = null)
        {
            target.position = (offset != null ? (source.position - (offset.position - target.position)) : source.position);
        }

        /// <summary>
        /// Sets the target <see cref="Transform.rotation"/> to the source <see cref="Transform.rotation"/>.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoUpdateRotation(Transform source, Transform target, Transform offset = null)
        {
            target.rotation = (offset != null ? source.rotation * Quaternion.Inverse(offset.localRotation) : source.rotation);
        }

        /// <summary>
        /// Sets the target <see cref="Transform.localScale"/> to the source <see cref="Transform.localScale"/>.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoUpdateScale(Transform source, Transform target, Transform offset = null)
        {
            target.localScale = (offset != null ? (source.localScale - (offset.localScale - target.localScale)) : source.localScale);
        }
    }
}