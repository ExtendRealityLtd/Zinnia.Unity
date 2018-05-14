namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;

    /// <summary>
    /// Forces the transformations of the source <see cref="Transform"/> to match those of the target <see cref="Transform"/>.
    /// </summary>
    public class TransformSourceFollowActiveTarget : FollowModifier
    {
        /// <summary>
        /// Determines whether the <see cref="FollowModifier"/> should process all targets or just the first active target.
        /// </summary>
        /// <returns>Always <see langword="true"/>.</returns>
        public override bool ProcessFirstAndActiveOnly()
        {
            return true;
        }

        /// <summary>
        /// Sets the source <see cref="Transform.position"/> to the target <see cref="Transform.position"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        public override void UpdatePosition(Transform source, Transform target)
        {
            if (source != null && target != null)
            {
                source.position = target.position;
            }
            CachedSource = source;
            CachedTarget = target;
        }

        /// <summary>
        /// Sets the source <see cref="Transform.rotation"/> to the target <see cref="Transform.rotation"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        public override void UpdateRotation(Transform source, Transform target)
        {
            if (source != null && target != null)
            {
                source.rotation = target.rotation;
            }
            CachedSource = source;
            CachedTarget = target;
        }

        /// <summary>
        /// Sets the source <see cref="Transform.localScale"/> to the target <see cref="Transform.localScale"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        public override void UpdateScale(Transform source, Transform target)
        {
            if (source != null && target != null)
            {
                source.localScale = target.localScale;
            }
            CachedSource = source;
            CachedTarget = target;
        }
    }
}