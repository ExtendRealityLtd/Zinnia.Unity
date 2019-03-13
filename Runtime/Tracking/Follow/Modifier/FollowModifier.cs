namespace Zinnia.Tracking.Follow.Modifier
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Tracking.Follow.Modifier.Property;

    /// <summary>
    /// Composes the logic to modify the position, rotation and scale of a <see cref="Transform"/>.
    /// </summary>
    public class FollowModifier : MonoBehaviour
    {
        /// <summary>
        /// The modifier to change the scale.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public PropertyModifier ScaleModifier { get; set; }
        /// <summary>
        /// The modifier to change the rotation.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public PropertyModifier RotationModifier { get; set; }
        /// <summary>
        /// The modifier to change the position.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public PropertyModifier PositionModifier { get; set; }

        /// <summary>
        /// Emitted before the follow is modified.
        /// </summary>
        [DocumentedByXml]
        public ObjectFollower.FollowEvent Premodified = new ObjectFollower.FollowEvent();
        /// <summary>
        /// Emitted after the follow is modified.
        /// </summary>
        [DocumentedByXml]
        public ObjectFollower.FollowEvent Modified = new ObjectFollower.FollowEvent();

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

        /// <summary>
        /// The event data to emit before and after each property type has been modified.
        /// </summary>
        protected readonly ObjectFollower.EventData eventData = new ObjectFollower.EventData();

        /// <summary>
        /// Attempts to call each of the given <see cref="PropertyModifier"/> options to modify the target.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        [RequiresBehaviourState]
        public virtual void Modify(GameObject source, GameObject target, GameObject offset = null)
        {
            if (!ValidateCache(source, target, offset))
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