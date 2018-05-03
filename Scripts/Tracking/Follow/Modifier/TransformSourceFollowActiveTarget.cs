namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;

    /// <summary>
    /// The TransformSourceFollowActiveTarget will force the transformations of the source Transform to match those of the target Transform.
    /// </summary>
    public class TransformSourceFollowActiveTarget : FollowModifier
    {
        /// <summary>
        /// The ProcessFirstAndActiveOnly method determines whether the FollowModifier should process all targets or just the first active target.
        /// </summary>
        /// <returns>Always returns `true`.</returns>
        public override bool ProcessFirstAndActiveOnly()
        {
            return true;
        }

        /// <summary>
        /// The UpdatePosition method attempts to set the source Transform position to the target Transform position.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilise in the modification.</param>
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
        /// The UpdateRotation method attempts to set the source Transform rotation to the target Transform rotation.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilise in the modification.</param>
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
        /// The UpdateScale method attempts to set the source Transform scale to the target Transform scale.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilise in the modification.</param>
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