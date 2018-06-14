namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;

    /// <summary>
    /// Describes logic to modify the position, rotation and scale of a <see cref="Transform"/>.
    /// </summary>
    public abstract class FollowModifier : MonoBehaviour
    {
        /// <summary>
        /// The process logic for the target.
        /// </summary>
        public enum ProcessTarget
        {
            /// <summary>
            /// Process all targets.
            /// </summary>
            All,
            /// <summary>
            /// Only process the first active target.
            /// </summary>
            FirstActive
        }

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
        /// The mechanism of how to process the targets.
        /// </summary>
        public ProcessTarget ProcessType
        {
            get;
            protected set;
        } = ProcessTarget.All;

        /// <summary>
        /// Updates the source position based on the target position.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilize in the modification.</param>
        public virtual void UpdatePosition(Transform source, Transform target)
        {
            if (isActiveAndEnabled && ValidateCache(source, target))
            {
                DoUpdatePosition(source, target);
            }
        }

        /// Updates the source rotation based on the target rotation.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilize in the modification.</param>
        public virtual void UpdateRotation(Transform source, Transform target)
        {
            if (isActiveAndEnabled && ValidateCache(source, target))
            {
                DoUpdateRotation(source, target);
            }
        }

        /// Updates the source scale based on the target scale.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilize in the modification.</param>
        public virtual void UpdateScale(Transform source, Transform target)
        {
            if (isActiveAndEnabled && ValidateCache(source, target))
            {
                DoUpdateScale(source, target);
            }
        }

        /// <summary>
        /// Updates the source position based on the target position.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilize in the modification.</param>
        protected abstract void DoUpdatePosition(Transform source, Transform target);
        /// Updates the source rotation based on the target rotation.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilize in the modification.</param>
        protected abstract void DoUpdateRotation(Transform source, Transform target);
        /// Updates the source scale based on the target scale.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilize in the modification.</param>
        protected abstract void DoUpdateScale(Transform source, Transform target);

        /// <summary>
        /// Caches the given source <see cref="Transform"/> and target <see cref="Transform"/> and determines if the set cache is valid.
        /// </summary>
        /// <param name="source">The source to modify.</param>
        /// <param name="target">The target to utilize in the modification.</param>
        /// <returns><see langword="true"/> if the cache contains a valid source and target.</returns>
        protected virtual bool ValidateCache(Transform source, Transform target)
        {
            CachedSource = source;
            CachedTarget = target;
            return (CachedSource != null && CachedTarget != null);
        }
    }
}