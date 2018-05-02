namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;

    /// <summary>
    /// The TransformTargetsFollowSource will force the transformations of all the target Transforms to match those of the source Transform.
    /// </summary>
    public class TransformTargetsFollowSource : FollowModifier
    {
        /// <summary>
        /// The ProcessFirstAndActiveOnly method determines whether the FollowModifier should process all targets or just the first active target.
        /// </summary>
        /// <returns>Always returns `false`.</returns>
        public override bool ProcessFirstAndActiveOnly()
        {
            return false;
        }

        /// <summary>
        /// The UpdatePosition method attempts to set the target Transform position to the source Transform position.
        /// </summary>
        /// <param name="source">The source to utilise in the modification.</param>
        /// <param name="target">The target to modify.</param>
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
        /// The UpdateRotation method attempts to set the target Transform rotation to the source Transform rotation.
        /// </summary>
        /// <param name="source">The source to utilise in the modification.</param>
        /// <param name="target">The target to modify.</param>
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
        /// The UpdateScale method attempts to set the target Transform scale to the source Transform scale.
        /// </summary>
        /// <param name="source">The source to utilise in the modification.</param>
        /// <param name="target">The target to modify.</param>
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