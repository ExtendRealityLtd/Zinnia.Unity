namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;

    /// <summary>
    /// Modifies the source <see cref="Transform"/> data to equal the target <see cref="Transform"/> data.
    /// </summary>
    public class ModifyTransform : FollowModifier
    {
        /// <summary>
        /// Sets the source <see cref="Transform.position"/> to the target <see cref="Transform.position"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        public override void UpdatePosition(Transform source, Transform target)
        {
            CachedSource = source;
            CachedTarget = target;
            if (source != null && target != null)
            {
                source.position = target.position;
            }
        }

        /// <summary>
        /// Sets the source <see cref="Transform.rotation"/> to the target <see cref="Transform.rotation"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        public override void UpdateRotation(Transform source, Transform target)
        {
            CachedSource = source;
            CachedTarget = target;
            if (source != null && target != null)
            {
                source.rotation = target.rotation;
            }
        }

        /// <summary>
        /// Sets the source <see cref="Transform.localScale"/> to the target <see cref="Transform.localScale"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        public override void UpdateScale(Transform source, Transform target)
        {
            CachedSource = source;
            CachedTarget = target;
            if (source != null && target != null)
            {
                source.localScale = target.localScale;
            }
        }
    }
}