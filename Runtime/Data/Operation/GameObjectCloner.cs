namespace Zinnia.Data.Operation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Duplicates a <see cref="GameObject"/> by cloning it.
    /// </summary>
    public class GameObjectCloner : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }

        /// <summary>
        /// The object to clone.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Source { get; set; }
        /// <summary>
        /// An optional object to parent the cloned objects to.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Parent { get; set; }

        /// <summary>
        /// Emitted when an object has been cloned.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Cloned = new UnityEvent();

        /// <summary>
        /// Duplicates <see cref="Source"/> by cloning it and optionally parents the cloned object to <see cref="Parent"/>.
        /// </summary>
        /// <returns>The cloned object, or <see langword="null"/> if no clone has been created.</returns>
        [RequiresBehaviourState]
        public virtual GameObject Clone()
        {
            return Clone(Source);
        }

        /// <summary>
        /// Duplicates a <see cref="GameObject"/> by cloning it and optionally parents the cloned object to <see cref="Parent"/>.
        /// </summary>
        /// <param name="source">The object to clone.</param>
        /// <returns>The cloned object, or <see langword="null"/> if no clone has been created.</returns>
        [RequiresBehaviourState]
        public virtual GameObject Clone(GameObject source)
        {
            if (source == null)
            {
                return null;
            }

            GameObject clone = Instantiate(source, Parent == null ? null : Parent.transform);
            Cloned?.Invoke(clone);
            return clone;
        }

        /// <summary>
        /// Duplicates <see cref="Source"/> by cloning it and parents the cloned object to <see cref="Parent"/>.
        /// </summary>
        public virtual void DoClone()
        {
            Clone();
        }

        /// <summary>
        /// Duplicates <see cref="Source"/> by cloning it and parents the cloned object to <see cref="Parent"/>.
        /// </summary>
        /// <param name="source">The object to clone.</param>
        public virtual void DoClone(GameObject source)
        {
            Clone(source);
        }
    }
}
