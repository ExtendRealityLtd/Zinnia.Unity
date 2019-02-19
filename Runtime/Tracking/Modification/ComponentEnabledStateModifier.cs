namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using System.Linq;
    using System.Collections.Generic;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.PropertyValidationMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Data.Attribute;
    using Zinnia.Data.Type;

    /// <summary>
    /// Provides the ability to modify the enabled state of a <see cref="Behaviour"/> or <see cref="Renderer"/> component.
    /// </summary>
    public class ComponentEnabledStateModifier : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="Object"/> types to manage the enabled state on.
        /// </summary>
        [TypePicker(typeof(Object)), DocumentedByXml]
        public List<SerializableType> types = new List<SerializableType>();

        /// <summary>
        /// The target to modify the enabled states for the provided <see cref="types"/>.
        /// </summary>
        [Serialized, Validated, Cleared]
        [field: DocumentedByXml]
        public GameObject Target { get; set; }

        /// <summary>
        /// Sets the enabled state of all matching <see cref="types"/> found on <see cref="Target"/>.
        /// </summary>
        /// <param name="state">The enabled state to apply.</param>
        [RequiresBehaviourState]
        public virtual void SetEnabledState(bool state)
        {
            if (Target == null)
            {
                return;
            }

            IEnumerable<Object> targetObjects = types.SelectMany(serializableType => Target.GetComponentsInChildren(serializableType, true));
            foreach (Object targetObject in targetObjects)
            {
                Behaviour potentialBehaviour = targetObject as Behaviour;
                if (potentialBehaviour != null)
                {
                    potentialBehaviour.enabled = state;
                    continue;
                }

                Renderer potentialRenderer = targetObject as Renderer;
                if (potentialRenderer != null)
                {
                    potentialRenderer.enabled = state;
                }
            }
        }
    }
}