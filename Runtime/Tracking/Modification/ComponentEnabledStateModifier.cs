namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Data.Type;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Provides the ability to modify the enabled state of a <see cref="Behaviour"/> or <see cref="Renderer"/> component.
    /// </summary>
    public class ComponentEnabledStateModifier : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="Object"/> types to manage the enabled state on.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public SerializableTypeComponentObservableList Types { get; set; }

        /// <summary>
        /// The target to modify the enabled states for the provided <see cref="Types"/>.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Target { get; set; }

        /// <summary>
        /// Sets the enabled state of all matching <see cref="Types"/> found on <see cref="Target"/>.
        /// </summary>
        /// <param name="state">The enabled state to apply.</param>
        [RequiresBehaviourState]
        public virtual void SetEnabledState(bool state)
        {
            if (Types == null || Target == null)
            {
                return;
            }

            foreach (SerializableType serializableType in Types.NonSubscribableElements)
            {
                foreach (Component targetObject in Target.GetComponentsInChildren(serializableType, true))
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
}