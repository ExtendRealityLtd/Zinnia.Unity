namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;

    /// <summary>
    /// The FollowModifier provides a way of describing logic to modify a Transform's position, rotation and scale.
    /// </summary>
    public abstract class FollowModifier : MonoBehaviour
    {
        /// The current source being used in the modifier process.
        public Transform CachedSource
        {
            get;
            protected set;
        }

        /// The current target being used in the modifier process.
        public Transform CachedTarget
        {
            get;
            protected set;
        }

        /// <summary>
        /// The ProcessFirstAndActiveOnly method determines whether the FollowModifier should process all targets or just the first active target.
        /// </summary>
        /// <returns>Returns `true` if only the first active target should be processed.</returns>
        public abstract bool ProcessFirstAndActiveOnly();
        /// <summary>
        /// The UpdatePosition method attempts to update the source position based on the target position.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilise in the modification.</param>
        public abstract void UpdatePosition(Transform source, Transform target);
        /// The UpdateRotation method attempts to update the source rotation based on the target rotation.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilise in the modification.</param>
        public abstract void UpdateRotation(Transform source, Transform target);
        /// The UpdateScale method attempts to update the source scale based on the target scale.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilise in the modification.</param>
        public abstract void UpdateScale(Transform source, Transform target);
    }
}