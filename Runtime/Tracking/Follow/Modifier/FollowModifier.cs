namespace Zinnia.Tracking.Follow.Modifier
{
    using UnityEngine;
    using Zinnia.Extension;
    using Zinnia.Tracking.Follow.Modifier.Property;

    /// <summary>
    /// Composes the logic to modify the position, rotation and scale of a <see cref="Transform"/>.
    /// </summary>
    public class FollowModifier : MonoBehaviour
    {
        [Tooltip("The modifier to change the scale.")]
        [SerializeField]
        private PropertyModifier scaleModifier;
        /// <summary>
        /// The modifier to change the scale.
        /// </summary>
        public PropertyModifier ScaleModifier
        {
            get
            {
                return scaleModifier;
            }
            set
            {
                scaleModifier = value;
            }
        }
        [Tooltip("The modifier to change the rotation.")]
        [SerializeField]
        private PropertyModifier rotationModifier;
        /// <summary>
        /// The modifier to change the rotation.
        /// </summary>
        public PropertyModifier RotationModifier
        {
            get
            {
                return rotationModifier;
            }
            set
            {
                rotationModifier = value;
            }
        }
        [Tooltip("The modifier to change the position.")]
        [SerializeField]
        private PropertyModifier positionModifier;
        /// <summary>
        /// The modifier to change the position.
        /// </summary>
        public PropertyModifier PositionModifier
        {
            get
            {
                return positionModifier;
            }
            set
            {
                positionModifier = value;
            }
        }

        /// <summary>
        /// Emitted before the follow is modified.
        /// </summary>
        public ObjectFollower.FollowEvent Premodified = new ObjectFollower.FollowEvent();
        /// <summary>
        /// Emitted after the follow is modified.
        /// </summary>
        public ObjectFollower.FollowEvent Modified = new ObjectFollower.FollowEvent();

        /// The current source being used in the modifier process.
        public GameObject CachedSource { get; protected set; }
        /// The current target being used in the modifier process.
        public GameObject CachedTarget { get; protected set; }
        /// <summary>
        /// The current being used as the offset in the modifier process.
        /// </summary>
        public GameObject CachedOffset { get; protected set; }

        /// <summary>
        /// The event data to emit before and after each property type has been modified.
        /// </summary>
        protected readonly ObjectFollower.EventData eventData = new ObjectFollower.EventData();

        /// <summary>
        /// Clears <see cref="ScaleModifier"/>.
        /// </summary>
        public virtual void ClearScaleModifier()
        {
            if (!this.IsValidState())
            {
                return;
            }

            ScaleModifier = default;
        }

        /// <summary>
        /// Clears <see cref="RotationModifier"/>.
        /// </summary>
        public virtual void ClearRotationModifier()
        {
            if (!this.IsValidState())
            {
                return;
            }

            RotationModifier = default;
        }

        /// <summary>
        /// Clears <see cref="PositionModifier"/>.
        /// </summary>
        public virtual void ClearPositionModifier()
        {
            if (!this.IsValidState())
            {
                return;
            }

            PositionModifier = default;
        }

        /// <summary>
        /// Attempts to call each of the given <see cref="PropertyModifier"/> options to modify the target.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        public virtual void Modify(GameObject source, GameObject target, GameObject offset = null)
        {
            if (!this.IsValidState() || !ValidateCache(source, target, offset))
            {
                return;
            }

            Premodified?.Invoke(eventData.Set(source, target, offset));

            if (ScaleModifier != null)
            {
                ScaleModifier.Modify(source, target, offset);
            }
            if (RotationModifier != null)
            {
                RotationModifier.Modify(source, target, offset);
            }
            if (PositionModifier != null)
            {
                PositionModifier.Modify(source, target, offset);
            }

            Modified?.Invoke(eventData.Set(source, target, offset));
        }

        /// <summary>
        /// Caches the given source and target then determines if the set cache is valid.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        /// <returns>Whether the cache contains a valid source and target.</returns>
        protected virtual bool ValidateCache(GameObject source, GameObject target, GameObject offset = null)
        {
            CachedSource = source;
            CachedTarget = target;
            CachedOffset = offset;
            return CachedSource != null && CachedTarget != null;
        }
    }
}