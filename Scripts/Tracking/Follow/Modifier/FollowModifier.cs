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
        /// The modifier to change the scale.
        /// </summary>
        [Tooltip("The modifier to change the scale.")]
        public PropertyModifier scaleModifier;
        /// <summary>
        /// The modifier to change the rotation.
        /// </summary>
        [Tooltip("The modifier to change the rotation.")]
        public PropertyModifier rotationModifier;
        /// <summary>
        /// The modifier to change the position.
        /// </summary>
        [Tooltip("The modifier to change the position.")]
        public PropertyModifier positionModifier;

        /// <summary>
        /// Emitted before the follow is modified.
        /// </summary>
        public ObjectFollower.UnityEvent Premodified = new ObjectFollower.UnityEvent();
        /// <summary>
        /// Emitted after the follow is modified.
        /// </summary>
        public ObjectFollower.UnityEvent Modified = new ObjectFollower.UnityEvent();

        /// The current source being used in the modifier process.
        public GameObject CachedSource
        {
            get;
            protected set;
        }

        /// The current target being used in the modifier process.
        public GameObject CachedTarget
        {
            get;
            protected set;
        }

        /// <summary>
        /// The current being used as the offset in the modifier process.
        /// </summary>
        public GameObject CachedOffset
        {
            get;
            protected set;
        }

        protected ObjectFollower.EventData eventData = new ObjectFollower.EventData();

        /// <summary>
        /// Attempts to call each of the given <see cref="PropertyModifier"/> options to modify the target.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        public virtual void Modify(GameObject source, GameObject target, GameObject offset = null)
        {
            if (!isActiveAndEnabled || !ValidateCache(source, target, offset))
            {
                return;
            }

            Premodified?.Invoke(eventData.Set(source, target, offset));

            if (scaleModifier != null)
            {
                scaleModifier.Modify(source, target, offset);
            }
            if (rotationModifier != null)
            {
                rotationModifier.Modify(source, target, offset);
            }
            if (positionModifier != null)
            {
                positionModifier.Modify(source, target, offset);
            }

            Modified?.Invoke(eventData.Set(source, target, offset));
        }

        /// <summary>
        /// Caches the given source and target then determines if the set cache is valid.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        /// <returns><see langword="true"/> if the cache contains a valid source and target.</returns>
        protected virtual bool ValidateCache(GameObject source, GameObject target, GameObject offset = null)
        {
            CachedSource = source;
            CachedTarget = target;
            CachedOffset = offset;
            return (CachedSource != null && CachedTarget != null);
        }
    }
}