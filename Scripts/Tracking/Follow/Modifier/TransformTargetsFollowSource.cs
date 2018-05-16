namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;

    /// <summary>
    /// Forces the transformations of all the target <see cref="Transform"/>s to match those of the source <see cref="Transform"/>.
    /// </summary>
    public class TransformTargetsFollowSource : FollowModifier
    {
        /// <summary>
        /// Determines whether the <see cref="FollowModifier"/> should process all targets or just the first active target.
        /// </summary>
        /// <returns>Always <see langword="false"/>.</returns>
        public override bool ProcessFirstAndActiveOnly()
        {
            return false;
        }

        /// <summary>
        /// Sets the target <see cref="Transform.position"/> to the source <see cref="Transform.position"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to utilize in the modification.</param>
        /// <param name="target">The target <see cref="Transform"/> to modify.</param>
        public override void UpdatePosition(Transform source, Transform target)
        {
            if (source != null && target != null)
            {
                target.position = source.position;
            }
            CachedSource = source;
            CachedTarget = target;
        }

        /// <summary>
        /// Sets the target <see cref="Transform.rotation"/> to the source <see cref="Transform.rotation"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to utilize in the modification.</param>
        /// <param name="target">The target <see cref="Transform"/> to modify.</param>
        public override void UpdateRotation(Transform source, Transform target)
        {
            if (source != null && target != null)
            {
                target.rotation = source.rotation;
            }
            CachedSource = source;
            CachedTarget = target;
        }

        /// <summary>
        /// Sets the target <see cref="Transform.localScale"/> to the source <see cref="Transform.localScale"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to utilize in the modification.</param>
        /// <param name="target">The target <see cref="Transform"/> to modify.</param>
        public override void UpdateScale(Transform source, Transform target)
        {
            if (source != null && target != null)
            {
                target.localScale = source.localScale;
            }
            CachedSource = source;
            CachedTarget = target;
        }
    }
}