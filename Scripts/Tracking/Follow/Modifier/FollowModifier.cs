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
        /// The current <see cref="Transform"/> being used as the offset in the modifier process.
        /// </summary>
        public Transform CachedOffset
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
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        public virtual void UpdatePosition(Transform source, Transform target, Transform offset = null)
        {
            if (isActiveAndEnabled && ValidateCache(source, target, offset))
            {
                DoUpdatePosition(source, target, offset);
            }
        }

        /// Updates the source rotation based on the target rotation.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        public virtual void UpdateRotation(Transform source, Transform target, Transform offset = null)
        {
            if (isActiveAndEnabled && ValidateCache(source, target, offset))
            {
                DoUpdateRotation(source, target, offset);
            }
        }

        /// Updates the source scale based on the target scale.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        public virtual void UpdateScale(Transform source, Transform target, Transform offset = null)
        {
            if (isActiveAndEnabled && ValidateCache(source, target, offset))
            {
                DoUpdateScale(source, target, offset);
            }
        }

        /// <summary>
        /// Updates the source position based on the target position.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected abstract void DoUpdatePosition(Transform source, Transform target, Transform offset = null);
        /// Updates the source rotation based on the target rotation.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected abstract void DoUpdateRotation(Transform source, Transform target, Transform offset = null);
        /// Updates the source scale based on the target scale.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected abstract void DoUpdateScale(Transform source, Transform target, Transform offset = null);

        /// <summary>
        /// Caches the given source <see cref="Transform"/> and target <see cref="Transform"/> and determines if the set cache is valid.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        /// <returns><see langword="true"/> if the cache contains a valid source and target.</returns>
        protected virtual bool ValidateCache(Transform source, Transform target, Transform offset = null)
        {
            CachedSource = source;
            CachedTarget = target;
            CachedOffset = offset;
            return (CachedSource != null && CachedTarget != null);
        }
    }
}