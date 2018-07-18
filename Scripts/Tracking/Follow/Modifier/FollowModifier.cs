namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;
    using VRTK.Core.Tracking.Follow.Modifier.Property;

    /// <summary>
    /// Composes the logic to modify the position, rotation and scale of a <see cref="Transform"/>.
    /// </summary>
    public class FollowModifier : MonoBehaviour
    {
        /// <summary>
        /// The modifier to change the position.
        /// </summary>
        [Tooltip("The modifier to change the position.")]
        public PropertyModifier positionModifier;
        /// <summary>
        /// The modifier to change the rotation.
        /// </summary>
        [Tooltip("The modifier to change the rotation.")]
        public PropertyModifier rotationModifier;
        /// <summary>
        /// The modifier to change the scale.
        /// </summary>
        [Tooltip("The modifier to change the scale.")]
        public PropertyModifier scaleModifier;

        /// <summary>
        /// Emitted before the follow is modified.
        /// </summary>
        public ObjectFollow.UnityEvent Premodified = new ObjectFollow.UnityEvent();
        /// <summary>
        /// Emitted after the follow is modified.
        /// </summary>
        public ObjectFollow.UnityEvent Modified = new ObjectFollow.UnityEvent();

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

        protected ObjectFollow.EventData eventData = new ObjectFollow.EventData();

        /// <summary>
        /// Attempts to call each of the given <see cref="PropertyModifier"/> options to modify the target <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        public virtual void Modify(Transform source, Transform target, Transform offset = null)
        {
            if (!isActiveAndEnabled || !ValidateCache(source, target, offset))
            {
                return;
            }

            Premodified?.Invoke(eventData.Set(source, target, offset));

            positionModifier?.Modify(source, target, offset);
            rotationModifier?.Modify(source, target, offset);
            scaleModifier?.Modify(source, target, offset);

            Modified?.Invoke(eventData.Set(source, target, offset));
        }

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