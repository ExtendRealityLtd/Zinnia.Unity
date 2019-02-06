namespace Zinnia.Tracking.Follow.Modifier.Property
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.PropertyValidationMethod;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Tracking.Follow;

    public abstract class PropertyModifier : MonoBehaviour
    {
        /// <summary>
        /// Determines whether the offset will be applied on the modification.
        /// </summary>
        [Serialized, Validated]
        [field: DocumentedByXml]
        public bool applyOffset { get; set; } = true;

        /// <summary>
        /// Emitted before the property is modified.
        /// </summary>
        [DocumentedByXml]
        public ObjectFollower.UnityEvent Premodified = new ObjectFollower.UnityEvent();
        /// <summary>
        /// Emitted after the property is modified.
        /// </summary>
        [DocumentedByXml]
        public ObjectFollower.UnityEvent Modified = new ObjectFollower.UnityEvent();

        protected ObjectFollower.EventData eventData = new ObjectFollower.EventData();

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

            offset = (applyOffset ? offset : null);

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