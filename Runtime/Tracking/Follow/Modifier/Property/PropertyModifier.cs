namespace Zinnia.Tracking.Follow.Modifier.Property
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Tracking.Follow;

    public abstract class PropertyModifier : MonoBehaviour
    {
        /// <summary>
        /// Determines whether the offset will be applied on the modification.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool ApplyOffset { get; set; } = true;

        /// <summary>
        /// Emitted before the property is modified.
        /// </summary>
        [DocumentedByXml]
        public ObjectFollower.FollowEvent Premodified = new ObjectFollower.FollowEvent();
        /// <summary>
        /// Emitted after the property is modified.
        /// </summary>
        [DocumentedByXml]
        public ObjectFollower.FollowEvent Modified = new ObjectFollower.FollowEvent();

        /// <summary>
        /// The event data to emit before and after the property has been modified.
        /// </summary>
        protected readonly ObjectFollower.EventData eventData = new ObjectFollower.EventData();

        /// <summary>
        /// Attempts modify the target.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        [RequiresBehaviourState]
        public virtual void Modify(GameObject source, GameObject target, GameObject offset = null)
        {
            if (source == null || target == null)
            {
                return;
            }

            offset = ApplyOffset ? offset : null;

            Premodified?.Invoke(eventData.Set(source, target, offset));
            DoModify(source, target, offset);
            Modified?.Invoke(eventData.Set(source, target, offset));
        }

        /// <summary>
        /// Attempts modify the target.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected abstract void DoModify(GameObject source, GameObject target, GameObject offset = null);
    }
}