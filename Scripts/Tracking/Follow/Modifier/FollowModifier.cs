namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;

    /// <summary>
    /// Describes logic to modify the position, rotation and scale of a <see cref="Transform"/>.
    /// </summary>
    public abstract class FollowModifier : MonoBehaviour
    {
        /// The current source <see cref="Transform"/> being used in the modifier process.
        public Transform CachedSource
        {
            get;
            protected set;
        }

        /// The current target <see cref="Transform"/> being used in the modifier process.
        public Transform CachedTarget
        {
            get;
            protected set;
        }

        /// <summary>
        /// Determines whether the <see cref="FollowModifier"/> should process all targets or just the first active target.
        /// </summary>
        /// <returns><see langword="true"/> if only the first active target should be processed.</returns>
        public abstract bool ProcessFirstAndActiveOnly();
        /// <summary>
        /// Updates the source position based on the target position.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilize in the modification.</param>
        public abstract void UpdatePosition(Transform source, Transform target);
        /// Updates the source rotation based on the target rotation.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilize in the modification.</param>
        public abstract void UpdateRotation(Transform source, Transform target);
        /// Updates the source scale based on the target scale.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilize in the modification.</param>
        public abstract void UpdateScale(Transform source, Transform target);
    }
}